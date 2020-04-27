#!/usr/bin/env python3
import flask
from flask import jsonify

app = flask.Flask(__name__)
app.config["DEBUG"] = True


@app.route('/', methods=['GET'])
def root():
    return "<i>it</i> <b>works!</b>"

@app.route('/data', methods=['GET'])
def more_data():
    return jsonify({
        'more': 'data'
    })

app.run()


