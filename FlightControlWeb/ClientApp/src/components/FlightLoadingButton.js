import React, { useState, useEffect, useContext, useCallback } from "react";

export default function FlightLoadingButton(props) {
    const [file, setFile] = useState(null);

    async function readFile(file) {
        return new Promise((resolve) => {
            const reader = new FileReader();
            reader.addEventListener('load', (event) => {
                resolve(event.target.result);
            });
            reader.readAsDataURL(file);
        });
    }

    const onChangeHandler = useCallback(async (event) => {
        const [file] = event.target.files;
        setFile(file.name);
        const fileContent = await readFile(file);

        const [, base64JSONFile] = fileContent.split('base64,');
        const json = atob(base64JSONFile);

        try {
            const flightPlan = JSON.parse(json);

            console.log(flightPlan);
            const response = await fetch('/api/FlightPlan', {
                method: 'POST',
                body: JSON.stringify(flightPlan),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            return await response.json();
        } catch (error) {
            // handle bad json file error
            console.error(error);
        }
    }, []);

    return (
        <div class="input-group">
            <div class="custom-file">
                <input type="file"
                    name="file"
                    accept="application/json"
                    onChange={(event) => onChangeHandler(event)}
                    onClick={(event) => { event.target.value = null }}
                    className={`custom-file-input`}
                    id="inputGroupFile01"
                    ariaDescribedby="inputGroupFileAddon01"
                    title="Click to select a file"
                />
                <label className="custom-file-label" htmlFor="inputGroupFile01">
                    {file ? file : 'Choose file'}
                </label>
            </div>
        </div>
    );
}