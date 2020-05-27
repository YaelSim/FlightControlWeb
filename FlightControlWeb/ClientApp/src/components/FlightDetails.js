import React, { useEffect, useState, useContext } from "react";
import FlightIdContext from "../contexts/FlightIdContext.js"
import styles from './FlightDetails.module.css';

export default function FlightDetails(props) {

    const [loading, setLoading] = useState(false);
    const [arrivalTime, setArrivalTime] = useState();

    async function getFlightPlan(id) {
        const r = await fetch(`/api/FlightPlan/${id}`);
        return await r.json();
    }

    useEffect(() => {
        if (props.flightPlan) {
            const timeToAdd = props.flightPlan.segments.map(segment => segment.timespan_seconds).reduce((a, b) => a + b, 0);
            console.log(timeToAdd);
            let flightDate = new Date(props.flightPlan.initial_location.date_time);
            flightDate.setSeconds(flightDate.getSeconds() + timeToAdd);
            setArrivalTime(flightDate);
        }
    }, [props.flightPlan]);

    if (loading) {
        return (
            <div className={styles['loader-container']}>
                <div className="spinner-border" role="status">
                    <span className="sr-only">Loading...</span>
                </div>
            </div>
        );
    }

    const dateToString = (date) => {
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    } 

    return (
        <div class="container">
            <div class="row mt-2">

                <div class="col-3">Company Name: </div>
                <div class="col-3">{props.flightPlan ? props.flightPlan.company_name : "-"}</div>
                <div class="col-3">Number Of Passengers: </div>
                <div class="col-3">{props.flightPlan ? props.flightPlan.passengers : "-"}</div>
            </div>
            <div class="row mt-3">
                <div class="col-3">Destionation: </div>
                <div class="col-3">{props.flightPlan
                    ? props.flightPlan.segments[props.flightPlan.segments.length - 1].latitude : "-"}
                    {props.flightPlan
                        ? ", " : ""}
                    {props.flightPlan
                        ? props.flightPlan.segments[props.flightPlan.segments.length - 1].longitude : ""}
                </div>
                <div class="col-3">Arrival Time: </div>
                <div class="col-3">{(props.flightPlan && arrivalTime) ? dateToString(arrivalTime) : "-"}</div>
            </div>
            <div class="row mt-3">
                <div class="col-3">Origin: </div>
                <div class="col-3">{props.flightPlan
                    ? props.flightPlan.initial_location.latitude : "-"}
                    {props.flightPlan
                        ? ", " : ""}
                    {props.flightPlan
                        ? props.flightPlan.initial_location.longitude : ""}</div>
                <div class="col-3">Departure Time: </div>
                <div class="col-3">{props.flightPlan ? props.flightPlan.initial_location.date_time : "-"}</div>
            </div>
        </div>


    );
}
