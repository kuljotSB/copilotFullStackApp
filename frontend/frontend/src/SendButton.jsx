import {useState} from "react";
import "./SendButton.css";

export default function SendButton({functionToCall}) {
    return(
        <button onClick = {functionToCall} className = "sendButton" >Send</button>
    );
}
