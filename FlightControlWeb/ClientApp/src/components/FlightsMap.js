import React, { useState } from "react";
import { Map, TileLayer } from "react-leaflet";
import L from 'leaflet';

export default function FlightsMap(props) {
    return (
        <Map center={[31.046051, 34.851612]} zoom={5} style={{ width: "100%", height: "500px" }}>
            <TileLayer
                attribution='&amp;copy <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
        </Map>
    );
}