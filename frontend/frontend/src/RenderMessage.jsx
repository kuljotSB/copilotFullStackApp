import { useState } from "react";
import "./RenderMessage.css";

export default function RenderMessage({ chatConvo }) {

    if (!chatConvo || !Array.isArray(chatConvo)) {
        console.log("not array");  // Return null or an appropriate fallback component
    }
    else
    {
    return (
      <div className="message-container">
        {chatConvo.map((item, index) => (
          <div key={index}>
            {item.userQuery && (<p className="userQuery" id = "userQuery">User Query: {item.userQuery}</p>)}
            {item.chatResponse && (<p className="chatResponse" id="chatResponse">Chat Response: {item.chatResponse}</p>)}
          </div>
        ))}
      </div>
    );
  }
}