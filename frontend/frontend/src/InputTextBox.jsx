import { useState } from "react";
import "./InputTextBox.css";
import axios from "axios";
import SendButton from "./SendButton";

export default function InputTextBox({setChatConvo , webSearchChoice, AISearchChoice, graphPluginChoice}) {
    const[userChildQuery, setChildUserQuery] = useState("");
    const[loadingStatus, setLoadingStatus] = useState(false);
    
    const handleChange = (e) => {
        setChildUserQuery((oldvalue) => e.target.value);
        console.log(userChildQuery);
    }

    const handleClick = () => {
        setLoadingStatus(true);
        console.log("Web search plugin choice " + webSearchChoice);
        console.log("AI search plugin choice " + AISearchChoice);
        console.log("Graph plugin choice " + graphPluginChoice);
        alert("sending request to server");
        axios ({
            url : "http://localhost:5124/Chat",
            method:"POST",
            data : {
                
                    "id":"1",
                    "WebSearcherPluginChoice":webSearchChoice,
                    "AISearchPluginChoice":AISearchChoice,
                    "GraphPluginChoice":graphPluginChoice,
                    "userQueryString":userChildQuery
                

            }

    })
    .then((res) => {
        setLoadingStatus(false);
        const chatEngineResponse =  res.data[0]?.chatResponse || "No response available";
        const serializedPlan = res.data[0].serializedPlan ;
        console.log(serializedPlan);
        setChatConvo((prevChatConvo) => [
            ...prevChatConvo,
            { userQuery: userChildQuery, chatResponse: chatEngineResponse, plannerMessage: serializedPlan}
        ]);
    })
    .catch((err) => {
        alert("did not find suitable plugin to handle the request");
        console.log(err);
        setLoadingStatus(false);
    })
    }
   if(!loadingStatus)
   {
    return (
        <div className="input-container">
         <div className = "send-button">
         <SendButton functionToCall = {handleClick} />
         </div>
            <input type="text" placeholder="enter your query" id="userQuery" value={userChildQuery} onChange={handleChange} />
           
        </div>
    )
}
else{
    return (
        <h1 id="loading" className="loading">Loading...</h1>
    )
}
}