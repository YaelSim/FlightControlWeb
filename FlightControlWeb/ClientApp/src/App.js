import React, { useState } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightIdContext from "./contexts/FlightIdContext.js";
import FlightsMap from "./components/FlightsMap.js";
//import './custom.css'

export default function App(props) {
    const [flightId, setFlightId] = useState(null);
    return (
        <FlightIdContext.Provider value={{ flightId, setFlightId }}>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-8 p-0">
                        <div className="row">
                            <div className="col-md-12">
                                <div style={{ height: "70vh" }}>
                                <FlightsMap/>
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-12">
                                <div className="card" style={{ height: "30vh"}}>
                                    <div className="card-body">
                                        <FlightDetails />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4 p-0" style={{ height: "100vh", overflow: "auto" }}>
                        <FlightList />
                    </div>
                </div>
            </div>
        </FlightIdContext.Provider>
    );
}
