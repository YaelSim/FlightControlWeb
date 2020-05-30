import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import 'leaflet/dist/leaflet.css';
import { ToastProvider } from 'react-toast-notifications'

const rootElement = document.getElementById('root');

ReactDOM.render(
  <ToastProvider placement = "top-center" autoDismiss >
    <App />
  </ToastProvider>,
  rootElement);


