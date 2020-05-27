import React, { useState, useEffect, useRef } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightIdContext from "./contexts/FlightIdContext.js";
import FlightsMap from "./components/FlightsMap.js";
import DragAndDrop from "./components/DragAndDrop.js"
import Dropzone, { useDropzone } from "react-dropzone"
import './custom.css';

const flightListStyles = {
    height: "100vh",
    overflow: "auto",
    boxShadow: '0px 0px 6px',
    zIndex: 9999
};

function useInterval(callback, delay) {
    const savedCallback = useRef();

    // Remember the latest callback.
    useEffect(() => {
        savedCallback.current = callback;
    }, [callback]);

    // Set up the interval.
    useEffect(() => {
        function tick() {
            savedCallback.current();
        }
        if (delay !== null) {
            let id = setInterval(tick, delay);
            return () => clearInterval(id);
        }
    }, [delay]);
}

export default function App(props) {
    const [flightPlan, setFlightPlan] = useState();
    const [flightId, setFlightId] = useState(null);
    const [flights, setFlights] = useState([]);

    async function getFlights() {
        const isoDateString = new Date().toISOString().replace(/\.\d*Z/, 'Z');
        const r = await fetch(`/api/Flights?relative_to=${isoDateString}&sync_all`);
        return await r.json();
    }

    // const fetchFlights = () => {
    //     getFlights().then(newFlights => {
    //         setFlights(newFlights);
    //         setTimeout(fetchFlights, 1000);
    //     });
    // }

    // useEffect(() => {
    //     fetchFlights();
    // }, []);

    useInterval(() => {
        getFlights().then(newFlights => {
            for (const newFlight of newFlights) {
                const previousFlight = flights &&
                    flights.find(flight => flight.flight_id === newFlight.flight_id);

                if (previousFlight) {
                    const x = newFlight.latitude - previousFlight.latitude;
                    const y = newFlight.longitude - previousFlight.longitude;
                    const angle = Math.atan2(y, x);
                    const degrees = 180 * angle / Math.PI;
                    const angleInDegrees = (360 + Math.round(degrees)) % 360;
                    newFlight.angle = angleInDegrees;
                }
            }

            setFlights(newFlights);
        }).catch(console.error);
    }, 3000);

    async function getFlightPlan(id) {
        const r = await fetch(`/api/FlightPlan/${id}`);
        return await r.json();
    }

    useEffect(() => {
        if (flightId) {
            getFlightPlan(flightId).then(flightPlan => {
                setFlightPlan(flightPlan);
            });
        } else {
            setFlightPlan(null);
        }
    }, [flightId]);

    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-md-8 p-0">
                    <div className="row">
                        <div className="col-md-12">
                            <div style={{ height: "70vh" }}>
                                <FlightsMap flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId} flightPlan={flightPlan} setFlightPlan={setFlightPlan} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="card" style={{ height: "30vh" }}>
                                <div className="card-body">
                                    <FlightDetails flightId={flightId} setFlightId={setFlightId} flightPlan={flightPlan} setFlightPlan={setFlightPlan} />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-md-4 p-0" style={flightListStyles}>
                    <FlightList flights={flights} setFlights={setFlights} flightId={flightId} setFlightId={setFlightId} />
                </div>
            </div>
        </div>
    );
}
