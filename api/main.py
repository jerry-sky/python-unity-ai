#!/usr/bin/env python3
import flask
from flask import jsonify
from flask_socketio import SocketIO, Namespace

from threading import Thread
from time import sleep

from model.field import Field
from model.start_simulation_request import start_simulation_request

FIELD = None
EMITTING_THREAD: Thread = None

# flask setup
app = flask.Flask(__name__)
app.config['DEBUG'] = True

# socketio setup
socketio = SocketIO(app, cors_allowed_origins='*')


# standard flask routes
@app.route('/', methods=['GET'])
def root():
    return '<i>it</i> <b>works!</b>'


@app.route('/data', methods=['GET'])
def more_data():
    return jsonify({
        'more': 'data'
    })


class EmittingThread(Thread):

    def __init__(self, to_run):
        Thread.__init__(self)
        self.alive = True
        self.__to_run = to_run

    def run(self):
        while self.alive:
            self.__to_run()
            sleep(1)


# socket.io event listeners
class SimulationNamespace(Namespace):

    def __init__(self, path: str):
        super().__init__(path)

    def on_connect(self):
        print('CONNECT')
        global FIELD, EMITTING_THREAD
        FIELD = None
        if EMITTING_THREAD is not None:
            EMITTING_THREAD.alive = False

    def on_disconnect(self):
        global FIELD, EMITTING_THREAD
        FIELD = None
        if EMITTING_THREAD is not None:
            EMITTING_THREAD.alive = False
        print('DISCONNECT')

    def on_start(self, data: start_simulation_request = None):
        global FIELD, EMITTING_THREAD
        if data is None:
            return

        if FIELD is not None:
            self.emit('DUMBASS')
            return

        FIELD = Field(data['width'], data['height'],
                             data['wolves_count'], data['rabbits_count'])

        def listener():
            f = FIELD
            w = []
            for wolf in f.wolves:
                w.append({
                    'x': wolf.pos[0],
                    'y': wolf.pos[1],
                    'alive': wolf.alive
                })

            r = []
            for rabbit in f.rabbits:
                r.append({
                    'x': rabbit.pos[0],
                    'y': rabbit.pos[1],
                    'alive': rabbit.alive
                })
            socketio.emit('field_update', {
                'rabbits': r,
                'wolves': w
            })

        EMITTING_THREAD = EmittingThread(listener)

        EMITTING_THREAD.start()

        FIELD.start_entities()


socketio.on_namespace(SimulationNamespace('/simulation'))

if __name__ == '__main__':
    socketio.run(app)
