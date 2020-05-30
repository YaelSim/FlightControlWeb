import React, { useState, useEffect } from "react";
import { Map, TileLayer, Marker } from "react-leaflet";
import L from 'leaflet';
import Curve from './Curve';
import 'leaflet-rotatedmarker';
const iconUrl = require('../images/coloredPlane.png');
const iconRetinaUrl = require('../images/coloredPlane.png');

export default function FlightsMap(props) {
    const [path, setPath] = useState([]);

    const iconAirplane = (flight) => {
        return new L.Icon({
            iconUrl: require('../images/plane.png'),
            iconRetinaUrl: require('../images/plane.png'),
            iconAnchor: null,
            popupAnchor: null,
            shadowUrl: null,
            shadowSize: null,
            shadowAnchor: null,
            iconSize: new L.point(50, 50),
            rotationAngle: 180,
        })
    };

    const coloredAirplane = (flight) => new L.Icon({
        iconUrl,
        iconRetinaUrl,
        iconAnchor: null,
        popupAnchor: null,
        shadowUrl: null,
        shadowSize: null,
        shadowAnchor: null,
        iconSize: new L.point(50, 50),
        rotationAngle: flight.angle || 0,
    });

    const clickHandler = (flightId) => {
        props.setFlightId(flightId);
    };

    useEffect(() => {
        if (props.flightPlan) {
            let pathOne = ['M', [props.flightPlan.initial_location.latitude, props.flightPlan.initial_location.longitude]]
            let pathTwo = props.flightPlan.segments
                .map((segment) => ('T', [segment.latitude, segment.longitude]))
            pathOne = [...pathOne, ...pathTwo];
            setPath(pathOne);
        }
    }, [props.flightPlan]);


    return (
        <Map onClick={() => { props.setFlightId(null) }} center={[31.046051, 34.851612]} zoom={5} style={{ width: "100%", height: "100%", padding: 0, margin: 0 }}>
            <TileLayer
                attribution='&amp;copy <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            {props.flights && props.flights
                .map(flight => (
                    <Marker
                        key={flight.flight_id}
                        position={[flight.latitude, flight.longitude]}
                        onClick={() => clickHandler(flight.flight_id)}
                        icon={flight.flight_id === props.flightId ? coloredAirplane(flight) : iconAirplane(flight)}
                    >
                    </Marker>
                ))}
            {props.flightId ? <Curve positions={path} option={{ color: '#ff85e5' }} /> : ""}
        </Map>
    );
}