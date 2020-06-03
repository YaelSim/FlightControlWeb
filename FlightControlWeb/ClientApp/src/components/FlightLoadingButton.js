import React, { useState, useCallback } from "react";
import styles from './FlightLoading.module.css';
import request from "../utils/request";
import { useToasts } from 'react-toast-notifications'

export default function FlightLoadingButton(props) {
    const [file, setFile] = useState(null);
    const { addToast } = useToasts()

    // Receive the file and convert it to be sent
    async function readFile(file) {
        return new Promise((resolve) => {
            const reader = new FileReader();
            reader.addEventListener('load', (event) => {
                resolve(event.target.result);
            });
            reader.readAsDataURL(file);
        });
    }

    // Execute browse click logic
    const onChangeHandler = useCallback(async (event) => {
        const [file] = event.target.files;
        setFile(file.name);
        const fileContent = await readFile(file);
        const [, base64JSONFile] = fileContent.split('base64,');
        const json = atob(base64JSONFile);

        let flightPlan;
        try {
            flightPlan = JSON.parse(json);

            const fields = [
                'passengers',
                'company_name',
                'initial_location',
                'segments',
            ];

            // Check that the flightPlan contains all the required fields
            const hasMissingFields = fields.some(field => !flightPlan[field]);
            const hasExtraFields = Object.keys(flightPlan).some(key => !fields.includes(key));

            // Check that there are no missing fields
            if (hasMissingFields) {
                throw new Error(`json ${file.name} has missing fields`);
            }

            // Check that there are no extra fields
            if (hasExtraFields) {
                throw new Error(`json ${file.name} has extra fields`);
            }

        } catch (error) {
            if (error && error.message) {
                addToast(error.message, { appearance: 'error' })
            } else {
                addToast("Adding flight failed, please try again later", { appearance: 'error' })
            }
            return;
        }

        //  Post request to the server
        try {
            await request('/api/FlightPlan', {
                method: 'POST',
                body: JSON.stringify(flightPlan),
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            // If flight added successfuly, print a message
            addToast('flight added successfully!', { appearance: 'success' });
        } catch (error) {
            if (error && error.message) {
                addToast(error.message, { appearance: 'error' })
            } else {
                addToast("Adding flight failed, please try again later", { appearance: 'error' })
            }
        }
    }, [addToast]);

    return (
        // Flight loading button design
        <div className="input-group">
            <div className="custom-file">
                <input type="file"
                    name="file"
                    accept="application/json"
                    onChange={(event) => onChangeHandler(event)}
                    onClick={(event) => { event.target.value = null }}
                    className={`custom-file-input`}
                    id="inputGroupFile01"
                    aria-describedby="inputGroupFileAddon01"
                    title="Click to select a file"
                />
                <label className="custom-file-label" htmlFor="inputGroupFile01">
                    <span className={styles['file-upload-text']}>
                        {file ? file : 'Choose file'}
                    </span>
                </label>
            </div>
        </div>
    );
}