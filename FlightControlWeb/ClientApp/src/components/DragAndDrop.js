import React from 'react';
import { useDropzone } from 'react-dropzone';

export default function DropAndDrop(props) {
    /* const {acceptedFiles, getRootProps, getInputProps} = useDropzone({accept: 'application/json'});
     
     const files = acceptedFiles.map(file => (
       <li key={file.path}>
         {file.path} - {file.size} bytes
       </li>
     ));*/

    return (
        <div className="file-upload-wrapper" >
            <input type="file" id="input-file-now" class="file-upload" />
        </div>
  );
}
