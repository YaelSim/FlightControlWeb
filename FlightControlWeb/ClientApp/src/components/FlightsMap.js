import React, { useState, useEffect, useContext } from "react";
import { Map, TileLayer, Marker } from "react-leaflet";
import L from 'leaflet';
import FlightIdContext from "../contexts/FlightIdContext.js";

export default function FlightsMap(props) {

    const iconAirplane = new L.Icon({
        iconUrl: require('../images/plane.png'),
        iconRetinaUrl: require('../images/plane.png'),
        iconAnchor: null,
        popupAnchor: null,
        shadowUrl: null,
        shadowSize: null,
        shadowAnchor: null,
        iconSize: new L.point(50, 50)
    });

    const coloredAirplane = new L.Icon({
        iconUrl: require('../images/coloredPlane.png'),
        iconRetinaUrl: require('../images/coloredPlane.png'),
        iconAnchor: null,
        popupAnchor: null,
        shadowUrl: null,
        shadowSize: null,
        shadowAnchor: null,
        iconSize: new L.point(50, 50)
    });

    const clickHandler = (flightId) => {
        props.setFlightId(flightId);
    };

    return (
        <Map onClick={() => {props.setFlightId(null)}} center={[31.046051, 34.851612]} zoom={5} style={{ width: "100%", height: "100%", padding: 0, margin: 0 }}>
            <TileLayer
                attribution='&amp;copy <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            {props.flights
                .map(flight => (
                    <Marker
                        position={[flight.latitude, flight.longitude]}
                        onClick={() => clickHandler(flight.flight_id)}
                        icon={flight.flight_id === props.flightId ? coloredAirplane : iconAirplane}
                    >
                    </Marker>
                ))}
        </Map>
    );
}