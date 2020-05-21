import React, { Component, useState } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightIdContext from "./contexts/FlightIdContext.js";
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
                                    <iframe
                                        src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d13520.621713958946!2d34.8734416!3d32.092084699999994!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sen!2sil!4v1590052999559!5m2!1sen!2sil"
                                        width="100%"
                                        height="100%"
                                        frameBorder={0}
                                        style={{ border: 0 }}
                                        allowFullScreen
                                        aria-hidden="false"
                                        tabIndex={0}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-12">
                                <div className="card" style={{ height: "30vh" }}>
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
