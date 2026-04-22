import tkinter as tk
from tkinter import messagebox, filedialog
import copy
import time
import json
import os
from typing import Optional, List, Set, Dict

class SudokuGame:
    """Ядро игры судоку (без Originator/Memento)"""
    def __init__(self, puzzle: List[List[int]]):
        self.grid = copy.deepcopy(puzzle)
        self.notes = [[set() for _ in range(9)] for _ in range(9)]
        self.initial_mask = [[cell != 0 for cell in row] for row in puzzle]
        self.mistakes = 0
        self.timer = 0.0
        self._start_time = time.time()

    def _is_valid(self, r: int, c: int, val: int) -> bool:
        for i in range(9):
            if i != c and self.grid[r][i] == val: return False
            if i != r and self.grid[i][c] == val: return False
        
        br, bc = 3 * (r // 3), 3 * (c // 3)
        for i in range(br, br + 3):
            for j in range(bc, bc + 3):
                if (i != r or j != c) and self.grid[i][j] == val: return False
        return True

    def set_cell(self, r: int, c: int, val: int) -> bool:
        if self.initial_mask[r][c] or not (1 <= val <= 9):
            return False
        if not self._is_valid(r, c, val):
            self.mistakes += 1
            return False
        self.grid[r][c] = val
        self.notes[r][c].clear()
        return True

    def clear_cell(self, r: int, c: int) -> bool:
        if self.initial_mask[r][c]: return False
        self.grid[r][c] = 0
        return True

class SaveManager:
    """Хранение «сырых» словарей вместо Memento"""
    def __init__(self):
        self.undo_stack: List[dict] = []  # dict вместо инкапсулированного объекта
        self.redo_stack: List[dict] = []

    def push(self, game: SudokuGame):
        # Дублирование логики копирования состояния
        game.timer = time.time() - game._start_time
        snapshot = {
            "grid": copy.deepcopy(game.grid),
            "notes": copy.deepcopy(game.notes),
            "initial_mask": copy.deepcopy(game.initial_mask),
            "mistakes": game.mistakes,
            "timer": game.timer
        }
        self.undo_stack.append(snapshot)
        self.redo_stack.clear()

    def undo(self) -> Optional[dict]:
        if len(self.undo_stack) > 1:
            prev = self.undo_stack.pop()
            self.redo_stack.append(prev)
            return self.undo_stack[-1]
        return None

    def redo(self) -> Optional[dict]:
        if self.redo_stack:
            nxt = self.redo_stack.pop()
            self.undo_stack.append(nxt)
            return nxt
        return None

class SudokuApp:
    """Графический интерфейс (без разделения ответственности)"""
    def __init__(self, root: tk.Tk):
        self.root = root
        self.root.title("Судоку (без паттерна)")
        self.root.geometry("420x520")
        self.root.resizable(False, False)

        puzzle = [
            [0,0,0,5,7,3,9,0,6], [3,9,6,1,0,0,0,0,5], [1,5,7,9,0,0,3,0,0],
            [0,0,0,6,9,2,7,0,0], [0,0,3,0,1,0,0,0,9], [0,2,8,0,0,4,0,5,1],
            [0,7,2,0,0,0,0,0,0], [5,0,0,2,8,7,0,6,3], [0,0,1,0,6,0,0,0,7]
        ]
        self.game = SudokuGame(puzzle)
        self.manager = SaveManager()
        
        #Прямое создание «сырого» снимка
        self.manager.push(self.game)

        self.vars: List[List[tk.StringVar]] = []
        self.entries: List[List[tk.Entry]] = []
        self._restoring = False

        self._build_ui()
        self._update_grid_ui()
        self._start_timer()

    def _build_ui(self):
        frame_grid = tk.Frame(self.root, bg="black", padx=2, pady=2)
        frame_grid.pack(pady=10)
        vcmd = (self.root.register(self._validate_input), '%P')

        for r in range(9):
            row_vars = []
            row_entries = []
            for c in range(9):
                var = tk.StringVar()
                entry = tk.Entry(frame_grid, textvariable=var, justify='center',
                                 font=('Consolas', 16, 'bold'), width=3,
                                 validate="key", validatecommand=vcmd)
                bg = "silver" if (r//3 + c//3) % 2 == 0 else "white"
                entry.config(bg=bg)
                entry.grid(row=r, column=c, padx=1, pady=1, ipady=4)
                var.trace_add("write", lambda *args, r=r, c=c: self._on_cell_input(r, c))
                row_vars.append(var)
                row_entries.append(entry)
            self.vars.append(row_vars)
            self.entries.append(row_entries)

        ctrl_frame = tk.Frame(self.root)
        ctrl_frame.pack(fill='x', padx=10, pady=5)
        tk.Button(ctrl_frame, text="<- Отмена", command=self._undo, width=10).pack(side='left', padx=2)
        tk.Button(ctrl_frame, text="-> Повтор", command=self._redo, width=10).pack(side='left', padx=2)

        self.status_var = tk.StringVar()
        tk.Label(self.root, textvariable=self.status_var, font=('Arial', 11), 
                 bg="#f8f9fa", anchor='w', padx=10).pack(fill='x', side='bottom', pady=5)

    def _validate_input(self, new_val: str) -> bool:
        return new_val == "" or new_val.isdigit() and 1 <= int(new_val) <= 9

    def _on_cell_input(self, r: int, c: int, *args):
        if self._restoring: return
        val_str = self.vars[r][c].get()
        old_val = self.game.grid[r][c]

        if val_str == "":
            self.game.clear_cell(r, c)
        else:
            val = int(val_str)
            if self.game.set_cell(r, c, val):
                pass
            else:
                self.vars[r][c].set(str(old_val) if old_val else "")
                messagebox.showwarning("Внимание", "Исходную ячейку нельзя менять!")
                return

        #Прямое сохранение состояния в менеджер
        self.manager.push(self.game)
        self._update_status()

    def _update_grid_ui(self):
        self._restoring = True
        for r in range(9):
            for c in range(9):
                val = self.game.grid[r][c]
                self.vars[r][c].set(str(val) if val else "")
                if self.game.initial_mask[r][c]:
                    self.entries[r][c].config(state='disabled', fg='#2c3e50')
                else:
                    self.entries[r][c].config(state='normal', fg='#2980b9')
        self._restoring = False
        self._update_status()

    def _undo(self):
        #Ручное восстановление из «сырого» словаря
        snapshot = self.manager.undo()
        if snapshot:
            self._restore_from_dict(snapshot)
            self._update_grid_ui()
        else:
            messagebox.showinfo("Информация", "Дальше отменять некуда.")

    def _redo(self):
        snapshot = self.manager.redo()
        if snapshot:
            self._restore_from_dict(snapshot)
            self._update_grid_ui()

    #Дублирование логики восстановления состояния
    def _restore_from_dict(self, data: dict) -> None:
        """Ручное восстановление полей из словаря."""
        self.game.grid = copy.deepcopy(data["grid"])
        self.game.notes = copy.deepcopy(data["notes"])
        self.game.initial_mask = copy.deepcopy(data["initial_mask"])
        self.game.mistakes = data["mistakes"]
        self.game.timer = data["timer"]
        self.game._start_time = time.time() - self.game.timer

    def _update_status(self):
        self.status_var.set(f" Ошибки: {self.game.mistakes} | Таймер: {self.game.timer:.1f}с")

    def _start_timer(self):
        self.game.timer = time.time() - self.game._start_time
        self._update_status()
        self.root.after(1000, self._start_timer)


if __name__ == "__main__":
    root = tk.Tk()
    app = SudokuApp(root)
    root.mainloop()
