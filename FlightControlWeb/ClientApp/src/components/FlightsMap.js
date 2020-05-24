import React, { useState, useEffect, useContext } from "react";
import { Map, Circle, TileLayer, LayersControl, FeatureGroup, Marker, Polyline } from "react-leaflet";
import L from 'leaflet';
import Curve from './Curve';

const pathOne = ['M', [50.14874640066278, 14.106445312500002],
    'Q', [51.67255514839676, 16.303710937500004],
    [50.14874640066278, 18.676757812500004],
    'T', [49.866316729538674, 25.0927734375]];

export default function FlightsMap(props) {
    const [flightPlan, setFlightPlan] = useState({});
    const [path, setPath] = useState([]);

    async function getFlightPlan(id) {
        const r = await fetch(`/api/FlightPlan/${id}`);
        return await r.json();
    }

    useEffect(() => {
        if (props.flightId) {
            getFlightPlan(props.flightId).then(flightPlan => {
                setFlightPlan(flightPlan);
            });
        }
    }, [props.flightId]);

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
        console.log(flightId);

    };

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
            <Curve positions={pathOne} option={{ color: 'red' }} />
        </Map>
    );
}