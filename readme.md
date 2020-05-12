# Python Unity AI Project

This project is an educational assignment for the subject of Selected Programming Language (Python).

**Wrocław University of Technology**\
**Faculty of Fundemental Problems of Technology**\
**Department of Fundamentals of Computer Science**

## API

[Install `pipenv`][pipenv-install] as this is the preferred package manager for this project.

[pipenv-install]: https://github.com/pypa/pipenv#installation

Whilst inside the `api` directory run
```bash
pipenv sync
```
to install all necessary packages for running the API Python server.

### Running the app

Currently the API server is in testing and rapid development.

Before running the Python app, (whilst inside the `api` directory) run
```bash
pipenv shell
```
to enter current project environment. **This will ensure the packages
installed for this project are made available for the app to run.**

Run `./main.py` from the `api` directory to start the `flusk` web application.\
*Please note: every time a change is made to any of these API files the server restarts.*
