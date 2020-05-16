#!usr/bin/python3

class Entity:
    def __init__(self, x, y):
        self.x, self.y = x, y


class Wolf(Entity):
    def __init__(self, x, y):
        super().__init__(x, y)

class Rabbit(Entity):
    def __init__(self, x, y):
        super().__init__(x, y)
