#!/usr/bin/env python3
import flask
from flask import jsonify
from flask_socketio import SocketIO, Namespace, emit

from threading import Thread

from model.field import Field
from model.start_simulation_request import start_simulation_request

# store the current state of the simulated field
# as this program will run only one simulation we need a single
# variable to store this simulation data
FIELD = None

# flask setup
app = flask.Flask(__name__)
app.config['DEBUG'] = True

# socketio setup
socketio = SocketIO(app, cors_allowed_origins='*',
                    logger=True, engineio_logger=True)


# Basic testing flask HTTP routes.

@app.route('/', methods=['GET'])
def root():
    return '<i>it</i> <b>works!</b>'


@app.route('/data', methods=['GET'])
def more_data():
    return jsonify({
        'more': 'data'
    })

# A direct connection between this server that runs the logic of
# the simulation and the client. This communication channel is defined
# by `socket.io`.


@socketio.on('connect')
def on_connect():
    """Wipe out the simulation on a new connection.
    """
    print('CONNECT')
    global FIELD
    FIELD = None


@socketio.on('disconnect')
def on_disconnect():
    """Wipe out the simulation on disconnect.
    """
    print('DISCONNECT')
    global FIELD
    FIELD = None


@socketio.on('start')
def on_start(data: start_simulation_request = None):
    """Start a new simulation and store it in a global variable.
    """
    global FIELD

    if data is None:
        return

    # create a new field for the simulation
    FIELD = Field(data['width'], data['height'],
                  data['wolves_count'], data['rabbits_count'])

    # let the entities move
    FIELD.start_entities()


@socketio.on('field_update')
def on_field_update(data=None):
    """Send requested field state.
    """

    f = FIELD

    # collect the data
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

    # send the data
    socketio.emit('field_update', {
        'rabbits': r,
        'wolves': w
    })


if __name__ == '__main__':
    socketio.run(app, port=5000)
