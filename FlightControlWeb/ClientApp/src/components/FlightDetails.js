import React, { useEffect, useState } from "react";
import styles from './FlightDetails.module.css';

export default function FlightDetails(props) {

    const [loading] = useState(false);
    const [arrivalTime, setArrivalTime] = useState();

    // Arrival time calculation
    useEffect(() => {
        if (props.flightPlan) {
            const timeToAdd = props.flightPlan.segments.map(segment => segment.timespan_seconds).reduce((a, b) => a + b, 0);
            let flightDate = new Date(props.flightPlan.initial_location.date_time);
            flightDate.setSeconds(flightDate.getSeconds() + timeToAdd);
            setArrivalTime(flightDate);
        }
    }, [props.flightPlan]);

    // Input loader design when needed
    if (loading) {
        return (
            <div className={styles['loader-container']}>
                <div className="spinner-border" role="status">
                    <span className="sr-only">Loading...</span>
                </div>
            </div>
        );
    }

    // Convert the date beautifly
    const dateToString = (date) => {
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    }

    return (
        // Flight details design
        <div className="container">
            <div className="row mt-2">
                <div className="col-3">Company Name: </div>
                <div className="col-3">{props.flightPlan ? props.flightPlan.company_name : "-"}</div>
                <div className="col-3">Number Of Passengers: </div>
                <div className="col-3">{props.flightPlan ? props.flightPlan.passengers : "-"}</div>
            </div>
            <div className="row mt-3">
                <div className="col-3">Destination: </div>
                <div className="col-3">{props.flightPlan
                    ? props.flightPlan.segments[props.flightPlan.segments.length - 1].latitude : "-"}
                    {props.flightPlan
                        ? ", " : ""}
                    {props.flightPlan
                        ? props.flightPlan.segments[props.flightPlan.segments.length - 1].longitude : ""}
                </div>
                <div className="col-3">Arrival Time: </div>
                <div className="col-3">{(props.flightPlan && arrivalTime) ? dateToString(arrivalTime) : "-"}</div>
            </div>
            <div className="row mt-3">
                <div className="col-3">Origin: </div>
                <div className="col-3">{props.flightPlan
                    ? props.flightPlan.initial_location.latitude : "-"}
                    {props.flightPlan
                        ? ", " : ""}
                    {props.flightPlan
                        ? props.flightPlan.initial_location.longitude : ""}</div>
                <div className="col-3">Departure Time: </div>
                <div className="col-3">{(props.flightPlan && props.flightPlan.initial_location.date_time) ?
                    dateToString(new Date(props.flightPlan.initial_location.date_time)) : "-"}</div>
            </div>
        </div>


    );
}