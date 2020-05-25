import React, { useState, useEffect } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightIdContext from "./contexts/FlightIdContext.js";
import FlightsMap from "./components/FlightsMap.js";
import DragAndDrop from "./components/DragAndDrop.js"
//import './custom.css'

const flightListStyles = { 
    height: "100vh", 
    overflow: "auto",
    boxShadow: '0px 0px 6px',
    zIndex: 9999,
};

export default function App(props) {
    const [flightPlan, setFlightPlan] = useState({});
    const [flightId, setFlightId] = useState(null);
    const [flights, setFlights] = useState([]);

    async function getFlights() {
        const r = await fetch('/api/Flights?relative_to=2020-05-21T16:32:22Z');
        return await r.json();
    }

    useEffect(() => {
        getFlights().then(flights => setFlights(flights));
    }, []);

    async function getFlightPlan(id) {
        const r = await fetch(`/api/FlightPlan/${id}`);
        return await r.json();
    }

    useEffect(() => {
        if (flightId) {
            getFlightPlan(flightId).then(flightPlan => {
                setFlightPlan(flightPlan);
            });
        }
    }, [flightId]);

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
                                <FlightsMap flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId} flightPlan={flightPlan} setFlightPlan={setFlightPlan}/>
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
                    <div className="col-md-4 p-0" style={flightListStyles} >
                      
                         <FlightList flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId} onItemDropped={dragHandler}/>
                    </div>
                </div>
            </div>
    );
}
