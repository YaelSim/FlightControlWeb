import React, { useState, useEffect } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightIdContext from "./contexts/FlightIdContext.js";
import FlightsMap from "./components/FlightsMap.js";
import DragAndDrop from "./components/DragAndDrop.js"
//import './custom.css'

export default function App(props) {
    const [flightId, setFlightId] = useState(null);
    const [flights, setFlights] = useState([]);

    async function getFlights() {
        const r = await fetch('/api/Flights');
        return await r.json();
    }

    useEffect(() => {
        getFlights().then(flights => setFlights(flights));
    }, []);

    const dragHandler = () => {
        console.log("drag");
    };

    return (
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-8 p-0">
                        <div className="row">
                            <div className="col-md-12">
                                <div style={{ height: "70vh" }}>
                                <FlightsMap flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId}/>
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-12">
                                <div className="card" style={{ height: "30vh"}}>
                                    <div className="card-body">
                                        <FlightDetails flightId={flightId} setFlightId={setFlightId}/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4 p-0" style={{ height: "100vh", overflow: "auto" } } >
                      
                         <FlightList flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId} onItemDropped={dragHandler}/>
                    </div>
                </div>
            </div>
    );
}
