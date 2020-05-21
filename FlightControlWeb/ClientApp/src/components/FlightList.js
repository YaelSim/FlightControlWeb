import React, { useState, useEffect } from "react";
export default function FlightDetails(props) {
    const [flights, setFlights] = useState([
        {
            flight_id: "1",
            longitude: "1",
            latitue: "1",
            passengers: "1",
            company_name: "1",
            date_time: "1",
            is_external: false
        },
        {
            flight_id: "2",
            longitude: "2",
            latitue: "2",
            passengers: "2",
            company_name: "2",
            date_time: "2",
            is_external: true
        },
        {
            flight_id: "3",
            longitude: "1",
            latitue: "1",
            passengers: "1",
            company_name: "1",
            date_time: "1",
            is_external: false
        }
    ]);

    const deleteRow = flight_id => {
        const copyFlights = [...flights];
        const index = copyFlights.findIndex(
            flight => flight.flight_id == flight_id
        );
        if (index != -1) {
            copyFlights.splice(index, 1);
            setFlights(copyFlights);
        }
    };

    return (
        <table className="table table-hover">
            <thead className="thead-light">
                <tr>
                    <th scope="col">Flight ID</th>
                    <th scope="col">Company</th>
                    <th scope="col">External</th>
                    <th scope="col" />
                </tr>
            </thead>
            <tbody>
                {flights
                    .filter(flight => !flight.is_external)
                    .map(flight => (
                        <tr>
                            <th scope="row">{flight.flight_id}</th>
                            <td>{flight.company_name}</td>
                            <td>No</td>
                            <td>
                                <svg
                                    className="bi bi-trash"
                                    onClick={() => deleteRow(flight.flight_id)}
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
                {flights
                    .filter(flight => flight.is_external)
                    .map(flight => (
                        <tr>
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
