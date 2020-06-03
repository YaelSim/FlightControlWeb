import React, { useState, useEffect, useRef, useCallback } from "react";
import FlightList from "./components/FlightList.js";
import FlightDetails from "./components/FlightDetails";
import FlightsMap from "./components/FlightsMap.js";
import { useToasts } from 'react-toast-notifications'
import request from "./utils/request";
import './custom.css';

const flightListStyles = {
    height: "100vh",
    overflow: "auto",
    boxShadow: '0px 0px 6px',
    zIndex: 9999
};

export default function App(props) {
    const [flightPlan, setFlightPlan] = useState();
    const [flightId, setFlightId] = useState(null);
    const [flights, setFlights] = useState([]);
    const { addToast } = useToasts();
    const flightIdRef = useRef(null);

    // Set current flightID
    useEffect(() => {
        flightIdRef.current = flightId;
    }, [flightId]);

    // Get flights request
    useEffect(() => {
        async function getFlights() {
            try {
                const isoDateString = new Date().toISOString().replace(/\.\d*Z/, 'Z');
                const flights = await request(`/api/Flights?relative_to=${isoDateString}&sync_all`);
                const lastFlightId = flightIdRef.current;
                const isSelectedFlightIdInFlights = lastFlightId && flights.some(flight => flight.flight_id === lastFlightId);

                // Change flightID to null, if flight was deleted
                if (!isSelectedFlightIdInFlights && lastFlightId !== null) {
                    setFlightId(null);
                }

                // Execute the next request, a second after the last one is over
                setFlights(flights);
                setTimeout(getFlights, 1000);
            } catch (error) {
                if (error && error.message) {
                    addToast(error.message, { appearance: 'error' })
                } else {
                    addToast("fetching flights failed, please try again later", { appearance: 'error' })
                }
            }
        }

        getFlights();
    }, [addToast]);

    // Flight plan by ID request
    const getFlightPlan = useCallback(async function (id) {
        try {
            return await request(`/api/FlightPlan/${id}`);
        } catch (error) {
            if (error && error.message) {
                addToast(error.message, { appearance: 'error' })
            } else {
                addToast("fetching flightPlan failed, please try again later", { appearance: 'error' })
            }
        }
    }, [addToast]);

    // If flightID exists, execute the get flight plan by ID request
    useEffect(() => {
        if (flightId) {
            getFlightPlan(flightId).then(flightPlan => {
                setFlightPlan(flightPlan);
            });
        } else {
            setFlightPlan(null);
        }
    }, [flightId, getFlightPlan]);

    return (
        // App design
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