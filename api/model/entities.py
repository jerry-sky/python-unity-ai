#!usr/bin/python3
from random import random, randint
from time import sleep
import threading
from copy import copy


class Entity(threading.Thread):
    def __init__(self, field, x, y):
        """ Place the entity on the given coordinates. """
        threading.Thread.__init__(self)
        self.pos = [x, y]
        self.field = field
        self.alive = True
        self.lock = threading.Lock()

    def run(self):
        while self.alive:
            self.lock.acquire()
            self.move()
            self.lock.release()

    def move(self):
        """ Make a move, notify the board, then wait for a moment. """
        prev_pos = copy(self.pos)
        self.random_move()
        self.field.notify(self, prev_pos)
        sleep(0.5 + random())

    def random_move(self):
        """ Move randomly. """
        valid_positions = self.field.get_valid_moves(self)
        if valid_positions != []:
            self.pos = valid_positions[randint(0, len(valid_positions)-1)]


class Wolf(Entity):
    def __init__(self, field, x, y):
        super().__init__(field, x, y)


class Rabbit(Entity):
    def __init__(self, field, x, y):
        super().__init__(field, x, y)

    def run(self):
        super().run()
        # delete after being attacked by a wolf
        self.field.rabbits.remove(self)
        self.join()
