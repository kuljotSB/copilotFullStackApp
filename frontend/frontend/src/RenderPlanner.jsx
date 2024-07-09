import {useState} from 'react';
import "./RenderPlanner.css";

export default function RenderPlanner({chatConvo})
{
    if (!chatConvo || !Array.isArray(chatConvo)) {
        console.log("not array");  // Return null or an appropriate fallback component
    }
    else
    {
    return (
      <div className="planner-container">
        {chatConvo.map((item, index) => (
          <div key={index}>
            {item.userQuery && (<p className="userQuery" id = "userQuery">User Query: {item.userQuery}</p>)}
            {item.chatResponse && (<p className="chatResponse" id="plannerMessage">Chat Response: {item.plannerMessage}</p>)}
          </div>
        ))}
      </div>
    );
}
}