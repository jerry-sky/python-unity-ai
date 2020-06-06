from typing_extensions import TypedDict


class StartSimulationRequest(TypedDict):
    """
    Defines what kind of data the server expects when a request
    to start the simulation is issued.
    """

    width: int
    height: int
    rabbits_count: int
    wolves_count: int
