import React, { useMemo, useState, useEffect, useCallback } from 'react';
import Dropzone, { useDropzone } from 'react-dropzone';

const baseStyle = {
  flex: 1,
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  padding: '20px',
  borderWidth: 2,
  borderRadius: 2,
  borderColor: '#eeeeee',
  borderStyle: 'dashed',
  backgroundColor: '#fafafa',
  color: '#bdbdbd',
  outline: 'none',
  transition: 'border .24s ease-in-out'
};

const activeStyle = {
  borderColor: '#2196f3'
};

const acceptStyle = {
  borderColor: '#00e676'
};

const rejectStyle = {
  borderColor: '#ff1744'
};

export default function DragAndDrop(props) {
  const [files, setfiles] = useState({});

  async function readFile(file) {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.addEventListener('load', (event) => {
        resolve(event.target.result);
      });
      reader.readAsDataURL(file);
    });
  }

  const onDrop = useCallback(async (files) => {
    const fileContent = await readFile(files[0]);
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

  // const {
  //   getRootProps,
  //   getInputProps,
  //   isDragActive,
  //   isDragAccept,
  //   isDragReject
  // } = useDropzone({ onDrop, accept: 'application/json' });
  // const style = useMemo(() => ({
  //   ...baseStyle,
  //   ...(isDragActive ? activeStyle : {}),
  //   ...(isDragAccept ? acceptStyle : {}),
  //   ...(isDragReject ? rejectStyle : {})
  // }), [
  //   isDragActive,
  //   isDragReject,
  //   isDragAccept
  // ]);

  return (
    // <div className="container" {...getRootProps({ style })}>
    //   <input {...getInputProps()} />
    //     {
    //       isDragActive ?
    //         <p style={{ height: '100%' }}>Drop the files here ...</p> :
    //         props.children
    //     }
    // </div>
    <Dropzone accept="application/json">
      {({
        getRootProps,
        getInputProps,
        isDragActive,
        isDragReject,
        isDragAccept,
      }) => {
        const style = {
          ...baseStyle,
          ...(isDragActive ? activeStyle : {}),
          ...(isDragAccept ? acceptStyle : {}),
          ...(isDragReject ? rejectStyle : {})
        };
        
        return (
          <div {...getRootProps({ style })}>
            <input {...getInputProps()} />
            <p>Drag 'n' drop some files here, or click to select files</p>
          </div>
        );
      }}
    </Dropzone>
  );
}