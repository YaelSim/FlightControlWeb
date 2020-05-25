import React, { useState, useEffect, useContext } from "react";
import { Map, Circle, TileLayer, LayersControl, FeatureGroup, Marker, Polyline } from "react-leaflet";
import L from 'leaflet';
import Curve from './Curve';
import styles from './FlightList.module.css'; 

export default function FlightsMap(props) {
    const [path, setPath] = useState([]);

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

    useEffect(() => {
        if (Object.keys(props.flightPlan).length !== 0) {
            let pathOne = ['M', [props.flightPlan.initial_location.latitude, props.flightPlan.initial_location.longitude]]
            let pathTwo = props.flightPlan.segments.
            map((segment) => ('T', [segment.latitude, segment.longitude]))
            pathOne = [...pathOne, ...pathTwo];
            console.log(pathOne);
            setPath(pathOne);

        }
    }, [props.flightPlan]);


    return (
        <Map onClick={() => { props.setFlightId(null) }} center={[31.046051, 34.851612]} zoom={5} style={{ width: "100%", height: "100%", padding: 0, margin: 0 }}>
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
<<<<<<< HEAD
            {props.flightId ? <Curve positions={path} option={{ color: "red" } } /> : ""}
=======
            {props.flightId ? <Curve positions={path} option={{ color: 'red' }} /> : ""}
>>>>>>> d47d6ea741e0c1f9f5f4f3e915b9a72fe72ccf11
        </Map>
    );
}