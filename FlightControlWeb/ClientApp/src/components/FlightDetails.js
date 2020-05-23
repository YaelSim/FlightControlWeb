import React, { useEffect, useState, useContext } from "react";
import FlightIdContext from "../contexts/FlightIdContext.js"

export default function FlightDetails() {
    const { flightId } = useContext(FlightIdContext);

    const [flightPlan, setFlightPlan] = useState({});
    const [loading, setLoading] = useState(false);

    async function getFlightPlan(id) {
        const r = await fetch(`/api/FlightPlan/${id}`);
        return await r.json();
    }

    useEffect(() => {
        if (flightId) {
            setLoading(true);
            getFlightPlan(flightId).then(flightPlan => {
                setFlightPlan(flightPlan);
                setLoading(false);
            });
        }
    }, [flightId]);

    if (loading) {
        return (
            <div class="spinner-border" role="status">
                <span class="sr-only">Loading...</span>
            </div>
        );
    }

    return (
        <div class="container">
      <div class="row mt-2">

        <div class="col-3">Company Name: </div>
        <div class="col-3">{flightId ? flightPlan.company_name : "-"}</div>
        <div class="col-3">Number Of Passengers: </div>
        <div class="col-3">{flightId ? flightPlan.passengers : "-"}</div>
      </div>
      <div class="row mt-3">
        <div class="col-3">Destionation: </div>
        <div class="col-3">-</div>
        <div class="col-3">Arrival Time: </div>
        <div class="col-3">-</div>
      </div>
      <div class="row mt-3">
        <div class="col-3">Origin: </div>
        <div class="col-3">-</div>
        <div class="col-3">Departure Time: </div>
        <div class="col-3">-</div>
      </div>
    </div>

        
    );
}
