import React, { useState, useEffect, useContext } from "react";
import FlightIdContext from "../contexts/FlightIdContext.js";
import styles from './FlightList.module.css'; 
import DragAndDrop from './DragAndDrop'

export default function FlightList(props) {

    async function deleteFlight(flightId) {
        try {
            const response = await fetch(`/api/Flights/${flightId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
            });
            return response.json(); // parses JSON response into native JavaScript objects
        } catch (error) {
            console.error(error);
        }
    }

    const deleteRow = (flight_id, e) => {
        e.stopPropagation();
        if (flight_id === props.flightId) {
            props.setFlightId(null);
        }
        deleteFlight(flight_id);
        const copyFlights = [...props.flights];
        const index = copyFlights.findIndex(
            flight => flight.flight_id === flight_id
        );
        if (index !== -1) {
            copyFlights.splice(index, 1);
            props.setFlights(copyFlights);
        }

    };

    const clickHandler = flightId => {
        props.setFlightId(flightId);
    };

    return (
        <table className="table table-hover">
            <thead className="thead">
                <tr>
                    <th scope="col">Flight ID</th>
                    <th scope="col">Company</th>
                    <th scope="col">External</th>
                    <th scope="col" />
                </tr>
            </thead>
            <tbody>
                {props.flights
                    .filter(flight => !flight.is_external)
                    .map(flight => (
                        <tr onClick={() => clickHandler(flight.flight_id)}
                            className={`${styles['row-hover']} ${flight.flight_id === props.flightId ? styles['selected-flight'] : ''}`}
                        >
                            <th scope="row">{flight.flight_id}</th>
                            <td>{flight.company_name}</td>
                            <td>No</td>
                            <td title="Delete Flight">
                                <svg
                                    className="bi bi-trash"
                                    onClick={e => deleteRow(flight.flight_id, e)}
                                    width="1em"
                                    height="1em"
                                    viewBox="0 0 16 16"
                                    fill="currentColor"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path d="M5.5 5.5A.5.5 0 016 6v6a.5.5 0 01-1 0V6a.5.5 0 01.5-.5zm2.5 0a.5.5 0 01.5.5v6a.5.5 0 01-1 0V6a.5.5 0 01.5-.5zm3 .5a.5.5 0 00-1 0v6a.5.5 0 001 0V6z" />
                                    <path
                                        fill-rule="evenodd"
                                        d="M14.5 3a1 1 0 01-1 1H13v9a2 2 0 01-2 2H5a2 2 0 01-2-2V4h-.5a1 1 0 01-1-1V2a1 1 0 011-1H6a1 1 0 011-1h2a1 1 0 011 1h3.5a1 1 0 011 1v1zM4.118 4L4 4.059V13a1 1 0 001 1h6a1 1 0 001-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"
                                        clip-rule="evenodd"
                                    />
                                </svg>
                            </td>
                        </tr>
                    ))}
                {props.flights
                    .filter(flight => flight.is_external)
                    .map(flight => (
                        <tr onClick={() => clickHandler(flight.flight_id)}>
                            <th scope="row">{flight.flight_id}</th>
                            <td>{flight.company_name}</td>
                            <td>Yes</td>
                            <td />
                        </tr>
                    ))}
            </tbody>
        </table>
    );
}
