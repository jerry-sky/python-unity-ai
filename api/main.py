#!/usr/bin/env python3
import flask
from flask import jsonify
from flask_socketio import SocketIO, Namespace, emit

# flask setup
app = flask.Flask(__name__)
app.config['DEBUG'] = True

# socketio setup
socketio = SocketIO(app, cors_allowed_origins='*')


# standard flask routes
@app.route('/', methods=['GET'])
def root():
    return "<i>it</i> <b>works!</b>"


@app.route('/data', methods=['GET'])
def more_data():
    return jsonify({
        'more': 'data'
    })

# socket.io event listeners
class TestingNamespace(Namespace):

    def on_connect(self):
        pass

    def on_disconnect(self):
        pass

    def on_my_event(self, data):
        print('--- MY EVENT OCCURRED ---')
        emit('my_response', {'data': 'yup, you\'re connected'})


socketio.on_namespace(TestingNamespace('/test'))

if __name__ == "__main__":
    socketio.run(app)
