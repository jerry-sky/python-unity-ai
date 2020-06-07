#!/usr/bin/python3
from model.entities import Wolf, Rabbit  # For some reason model.entities
from random import randint
from os import system
from typing import List
from time import sleep  # For testing purposes.

# A moving entity will consider squares occupied by
# classes in its list as valid move targets.
VALID_FOR = {
    Rabbit: [],
    Wolf: [Rabbit]
}


class Field:
    """
    Class used for storing and performing simulation related actions.
    """

    def __init__(self, width, height, n_wolves, n_rabbits):
        self.width, self.height = width, height
        sq = []
        for _ in range(width):
            sq.append([None] * height)
        self.sq = sq
        self.wolves, self.rabbits = [], []
        self.spawn_entities(n_wolves, n_rabbits)

    def spawn_entities(self, n_wolves, n_rabbits):
        i = 0
        while i < n_wolves:
            rand = [randint(0, self.width-1), randint(0, self.height-1)]
            if self.sq[rand[0]][rand[1]] is None:
                w = Wolf(self, rand[0], rand[1])
                self.wolves.append(w)
                self.sq[rand[0]][rand[1]] = w
                i += 1
        i = 0
        while i < n_rabbits:
            rand = [randint(0, self.width-1), randint(0, self.height-1)]
            if self.sq[rand[0]][rand[1]] is None:
                r = Rabbit(self, rand[0], rand[1])
                self.rabbits.append(r)
                self.sq[rand[0]][rand[1]] = r
                i += 1

    def start_entities(self):
        """ Launch all threads. """
        for w in self.wolves:
            w.start()
        for r in self.rabbits:
            r.start()

    def notify(self, entity, prev_pos):
        """ Function called by an entity after a move. """
        self.update_field(entity, prev_pos)

    def get_valid_moves(self, ent):
        """ Return a list of valid coordinates around an entity. """
        return [(i, j) for i in range(ent.pos[0] - 1, ent.pos[0] + 2)
                for j in range(ent.pos[1] - 1, ent.pos[1] + 2)
                if 0 <= i < self.width and 0 <= j < self.height
                and (self.sq[i][j] is None
                     or self.sq[i][j].__class__ in VALID_FOR[ent.__class__])
                ]

    def update_field(self, entity, prev_pos):
        self.sq[prev_pos[0]][prev_pos[1]] = None
        self.sq[entity.pos[0]][entity.pos[1]] = entity

    def print_field(self):
        """ Show the board in the terminal. """
        system('clear')
        f = self
        for y in range(f.height):
            row = ['[W]' if f.sq[x][y] in f.wolves
                   else '[R]' if f.sq[x][y] in f.rabbits
                   else '[ ]' for x in range(f.width)]
            print(''.join(row))


if __name__ == '__main__':
    # example program showing how this class can be used
    f = Field(10, 10, 3, 20)
    f.start_entities()
    while True:
        f.print_field()
        print(len(f.rabbits))
        print(len(f.wolves))
        sleep(0.5)
