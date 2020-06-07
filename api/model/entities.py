#!usr/bin/python3
from random import random, randint
from time import sleep
import threading
from copy import copy


class Entity(threading.Thread):
    """Represents a single entity being simulated.
    """

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
    """Represents a predator in the simulation.
    """
    def __init__(self, field, x, y):
        super().__init__(field, x, y)

    def move(self):
        """ Attempt to chase the nearest rabbit (not yet implemented). """
        if self.field.rabbits == []:
            # If no rabbits are present, use generic movement pattern.
            super().move()
        else:
            # Find the nearest rabbit.
            nearest = [self.field.rabbits[0], None]
            nearest[1] = (abs(nearest[0].pos[0] - self.pos[0])
                        + abs(nearest[0].pos[1] - self.pos[1]))
            for i in range(len(self.field.rabbits)):
                curr = self.field.rabbits[i]
                if (abs(curr.pos[0] - self.pos[0]) + abs(curr.pos[1] - self.pos[1])) < nearest[1]:
                    # Update the nearest target and the distance towards it.
                    nearest = [curr,
                            abs(curr.pos[0] - self.pos[0])
                            + abs(curr.pos[1] - self.pos[1])
                            ]
            prev_pos = copy(self.pos)
            self.move_towards(nearest[0])
            self.field.notify(self, prev_pos)
            caught_rabbit = [r for r in self.field.rabbits if r.pos == self.pos]
            if caught_rabbit != []:
                # Eat the caught rabbit.
                caught_rabbit[0].alive = False
            sleep(0.5 + random())

    def move_towards(self, target):
        valid_positions = self.field.get_valid_moves(self)
        closer_positions = []
        for i in range(len(self.pos)):
            step = target.pos[i] - self.pos[i]
            if step != 0:
                step = int(step/abs(step))
                new_pos = list(self.pos)
                new_pos[i] += step
                closer_positions.append(tuple(new_pos))
        # Add a diagonal move.
        combined = list(self.pos)
        for i in range(len(closer_positions)):
            combined[i] += closer_positions[i][i]
        combined = tuple(combined)
        valid_positions = [pos for pos in valid_positions if pos in closer_positions]
        if valid_positions != []:
            self.pos = valid_positions[randint(0, len(valid_positions)-1)]


class Rabbit(Entity):
    """Represents a prey in the simulation.
    """
    def __init__(self, field, x, y):
        super().__init__(field, x, y)

    def run(self):
        super().run()
        # Delete after being attacked by a wolf.
        self.field.rabbits.remove(self)
        exit()
