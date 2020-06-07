#!usr/bin/python3
from random import random, randint
from time import sleep
import threading
from copy import copy

move_delay = 0.5 + random()

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
        sleep(move_delay)

    def random_move(self):
        """ Move randomly. """
        valid_positions = self.field.get_valid_moves(self)
        if valid_positions != []:
            self.pos = valid_positions[randint(0, len(valid_positions)-1)]


class Wolf(Entity):
    def __init__(self, field, x, y):
        super().__init__(field, x, y)

    def move(self):
        """ Attempt to chase the nearest rabbit (not yet implemented). """
        prev_pos = copy(self.pos)
        self.random_move()
        self.field.notify(self, prev_pos)
        caught_rabbit = [r for r in self.field.rabbits if r.pos == self.pos]
        if caught_rabbit != []:
            # Eat the caught rabbit.
            caught_rabbit[0].alive = False
        sleep(move_delay)


class Rabbit(Entity):
    def __init__(self, field, x, y):
        super().__init__(field, x, y)

    def run(self):
        super().run()
        # Delete after being attacked by a wolf.
        self.field.rabbits.remove(self)
        exit()
