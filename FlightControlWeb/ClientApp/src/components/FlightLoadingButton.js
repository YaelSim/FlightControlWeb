import React, { useState, useEffect, useContext } from "react";
import { setSyntheticLeadingComments } from "typescript";

export default function FlightLoadingButton(props) {
    const [selectedFile, setSelectedFile] = useState(null);

    const onChangeHandler = event => {

        setSelectedFile(event.target.files[0])

    }

    return (
        <input type="file" name="file" onChange={() => onChangeHandler} />)
}