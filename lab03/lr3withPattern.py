import tkinter as tk
from tkinter import messagebox, filedialog
import copy
import time
import json
import os
from typing import Optional, List, Set, Dict

#классы, реализуюущие паттерн "хранитель"

class SudokuSnapshot:
    """Хранитель (Memento). Неподдающийся изменению снимок состояния."""
    __slots__ = ("_grid", "_notes", "_initial_mask", "_mistakes", "_timer")

    def __init__(self, grid, notes, initial_mask, mistakes, timer):
        self._grid = copy.deepcopy(grid)
        self._notes = copy.deepcopy(notes)
        self._initial_mask = copy.deepcopy(initial_mask)
        self._mistakes = mistakes
        self._timer = timer

    def _get_state(self) -> dict:
        """Внутренний метод. Доступен только SudokuGame."""
        return {
            "grid": copy.deepcopy(self._grid),
            "notes": copy.deepcopy(self._notes),
            "initial_mask": copy.deepcopy(self._initial_mask),
            "mistakes": self._mistakes,
            "timer": self._timer
        }


class SudokuGame:
    """Создатель (Originator). Ядро игры судоку."""
    def __init__(self, puzzle: List[List[int]]):
        self.grid = copy.deepcopy(puzzle)
        self.notes = [[set() for _ in range(9)] for _ in range(9)]
        self.initial_mask = [[cell != 0 for cell in row] for row in puzzle]
        self.mistakes = 0
        self.timer = 0.0
        self._start_time = time.time()

     #проверка правил судоку
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
        
        #если нарушены правила судоку, то увеличиваем счётчик ошибок
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

    def create_memento(self) -> 'SudokuSnapshot':
        self.timer = time.time() - self._start_time
        return SudokuSnapshot(self.grid, self.notes, self.initial_mask, self.mistakes, self.timer)

    def restore(self, snapshot: 'SudokuSnapshot') -> None:
        state = snapshot._get_state()
        self.grid = state["grid"]
        self.notes = state["notes"]
        self.initial_mask = state["initial_mask"]
        self.mistakes = state["mistakes"]
        self.timer = state["timer"]
        self._start_time = time.time() - self.timer

class SaveManager:
    """Опекун (Caretaker). Управление Undo/Redo"""
    def __init__(self):
        self.undo_stack: List[SudokuSnapshot] = []
        self.redo_stack: List[SudokuSnapshot] = []

    def push(self, snapshot: SudokuSnapshot):
        self.undo_stack.append(snapshot)
        self.redo_stack.clear()  #очистка redo при новом действии

    def undo(self) -> Optional[SudokuSnapshot]:
        if len(self.undo_stack) > 1:  #оставляем минимум 1 снимок (начальное состояние)
            prev = self.undo_stack.pop()
            self.redo_stack.append(prev)
            return self.undo_stack[-1]
        return None

    def redo(self) -> Optional[SudokuSnapshot]:
        if self.redo_stack:
            nxt = self.redo_stack.pop()
            self.undo_stack.append(nxt)
            return nxt
        return None

#класс графического интерфейса
class SudokuApp:
    def __init__(self, root: tk.Tk):
        self.root = root
        self.root.title("Судоку")
        self.root.geometry("420x520")
        self.root.resizable(False, False)

        #инициализация домена
        puzzle = [
            [0,0,0,5,7,3,9,0,6], [3,9,6,1,0,0,0,0,5], [1,5,7,9,0,0,3,0,0],
            [0,0,0,6,9,2,7,0,0], [0,0,3,0,1,0,0,0,9], [0,2,8,0,0,4,0,5,1],
            [0,7,2,0,0,0,0,0,0], [5,0,0,2,8,7,0,6,3], [0,0,1,0,6,0,0,0,7]
        ]
        self.game = SudokuGame(puzzle)
        self.manager = SaveManager()
        self.manager.push(self.game.create_memento())  #сохраняем начальное состояние

        self.vars: List[List[tk.StringVar]] = []
        self.entries: List[List[tk.Entry]] = []
        self._restoring = False  #флаг для подавления триггеров при загрузке

        self._build_ui()
        self._update_grid_ui()
        self._start_timer()

    def _build_ui(self):
        #сетка 9x9
        frame_grid = tk.Frame(self.root, bg="black", padx=2, pady=2)
        frame_grid.pack(pady=10)

        #валидатор ввода (только 1-9 или пусто)
        vcmd = (self.root.register(self._validate_input), '%P')

        for r in range(9):
            row_vars = []
            row_entries = []
            for c in range(9):
                var = tk.StringVar()
                entry = tk.Entry(frame_grid, textvariable=var, justify='center',
                                 font=('Consolas', 16, 'bold'), width=3,
                                 validate="key", validatecommand=vcmd)
                
                #стилизация 3x3 блоков
                bg = "silver" if (r//3 + c//3) % 2 == 0 else "white"
                entry.config(bg=bg)
                entry.grid(row=r, column=c, padx=1, pady=1, ipady=4)

                #привязка события изменения значения
                var.trace_add("write", lambda *args, r=r, c=c: self._on_cell_input(r, c))

                row_vars.append(var)
                row_entries.append(entry)
            self.vars.append(row_vars)
            self.entries.append(row_entries)

        #панель управления
        ctrl_frame = tk.Frame(self.root)
        ctrl_frame.pack(fill='x', padx=10, pady=5)

        tk.Button(ctrl_frame, text="↩️ Отмена", command=self._undo, width=10).pack(side='left', padx=2)
        tk.Button(ctrl_frame, text="↪️ Повтор", command=self._redo, width=10).pack(side='left', padx=2)

        #статус-бар
        self.status_var = tk.StringVar()
        tk.Label(self.root, textvariable=self.status_var, font=('Arial', 11), 
                 bg="#f8f9fa", anchor='w', padx=10).pack(fill='x', side='bottom', pady=5)

    def _validate_input(self, new_val: str) -> bool:
        return new_val == "" or new_val.isdigit() and 1 <= int(new_val) <= 9

    def _on_cell_input(self, r: int, c: int, *args):
        if self._restoring: return  #не создаём memento при программной синхронизации

        val_str = self.vars[r][c].get()
        old_val = self.game.grid[r][c]

        if val_str == "":
            self.game.clear_cell(r, c)
        else:
            val = int(val_str)
            if self.game.set_cell(r, c, val):
                pass  #успех
            else:
                # Ошибка: исходная ячейка или неверный ввод
                self.game.mistakes += 1
                self.vars[r][c].set(str(old_val) if old_val else "")
                messagebox.showwarning("Внимание", "Исходную ячейку нельзя менять!")
                return

        # ПРИМЕНЕНИЕ ПАТТЕРНА: сохраняем снимок после изменения
        self.manager.push(self.game.create_memento())
        self._update_status()

    def _update_grid_ui(self):
        """Синхронизирует View с Model (после Undo/Redo/Load)."""
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
        snap = self.manager.undo()
        if snap:
            self.game.restore(snap)
            self._update_grid_ui()
        else:
            messagebox.showinfo("Информация", "Дальше отменять некуда.")

    def _redo(self):
        snap = self.manager.redo()
        if snap:
            self.game.restore(snap)
            self._update_grid_ui()

    def _update_status(self):
        self.status_var.set(f"❌ Ошибки: {self.game.mistakes} | ⏱ {self.game.timer:.1f}с")

    def _start_timer(self):
        self._update_status()
        self.root.after(1000, self._start_timer)


if __name__ == "__main__":
    root = tk.Tk()
    app = SudokuApp(root)
    root.mainloop()
