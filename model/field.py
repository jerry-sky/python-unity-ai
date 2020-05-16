#!/usr/bin/python3
from entities import Wolf, Rabbit
from random import randint

class Field:
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
                w = Wolf(rand[0], rand[1])
                self.wolves.append(w)
                self.sq[rand[0]][rand[1]] = w
                i += 1
        i = 0
        while i < n_rabbits:
            rand = [randint(0, self.width-1), randint(0, self.height-1)]
            if self.sq[rand[0]][rand[1]] is None:
                r = Rabbit(rand[0], rand[1])
                self.rabbits.append(r)
                self.sq[rand[0]][rand[1]] = r
                i += 1


if __name__ == '__main__':
    f = Field(10, 12, 3, 15)
    for y in range(f.height):
        row = [ '[W]' if f.sq[x][y] in f.wolves else '[R]' if f.sq[x][y] in f.rabbits else '[ ]' for x in range(f.width) ]
        print(''.join(row))
