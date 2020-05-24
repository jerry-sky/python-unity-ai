#!/usr/bin/env python3
import flask
from flask import jsonify
from flask_socketio import SocketIO, Namespace, emit

from threading import Thread
from time import sleep

from model.field import Field
from model.start_simulation_request import start_simulation_request

FIELD = None

# flask setup
app = flask.Flask(__name__)
app.config['DEBUG'] = True

# socketio setup
socketio = SocketIO(app, cors_allowed_origins='*',
                    logger=True, engineio_logger=True)


# standard flask routes
@app.route('/', methods=['GET'])
def root():
    return '<i>it</i> <b>works!</b>'


@app.route('/data', methods=['GET'])
def more_data():
    return jsonify({
        'more': 'data'
    })


@socketio.on('connect')
def on_connect():
    print('CONNECT')
    global FIELD
    FIELD = None


@socketio.on('disconnect')
def on_disconnect():
    global FIELD
    FIELD = None
    print('DISCONNECT')


@socketio.on('start')
def on_start(data: start_simulation_request = None):
    global FIELD
    if data is None:
        return

    FIELD = Field(data['width'], data['height'],
                  data['wolves_count'], data['rabbits_count'])

    FIELD.start_entities()


@socketio.on('field_update')
def on_field_update(data=None):
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


if __name__ == '__main__':
    socketio.run(app, port=5000)
