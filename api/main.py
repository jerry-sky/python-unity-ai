#!/usr/bin/env python3
import flask
from flask import jsonify
from flask_socketio import SocketIO, Namespace

from model.field import Field
from model.start_simulation_request import start_simulation_request

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

# socket.io event listeners


class SimulationNamespace(Namespace):

    def __init__(self, path: str):
        super().__init__(path)

        self.__simulations = []

    def on_connect(self):
        pass

    def on_disconnect(self):
        pass

    def on_start(self, data: start_simulation_request = None):
        if data is None:
            return

        f = Field(data['width'], data['height'], data['wolves_count'], data['rabbits_count'])

        self.__simulations.append(f)

        def listener(rabbits, wolves):
            socketio.emit('field_update', {
                'rabbits': rabbits,
                'wolves': wolves
            })

        f.add_event_listener(listener)
        f.start_entities()


socketio.on_namespace(SimulationNamespace('/simulation'))

if __name__ == '__main__':
    socketio.run(app)
