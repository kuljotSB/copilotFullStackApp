import {useState} from "react";
import axios from "axios";
import "./GraphPluginButton.css";

export default function GraphPluginButton({setchoice})
{
    const [choice, setChildChoice] = useState(false);

    const EnableGraphPlugin = () => {
        const newChoice=true;
        alert("Enabling Graph Plugin. Go to console to see the logs.");
        console.log("Enabling Graph plugin");
        setChildChoice(true);
        setchoice(true);
        axios ({
            url : "http://localhost:5124/acquireaccesstoken",
            method:"GET"
        })
        .then((res) => {
            alert("graph validation successful");
        })
        .catch((err) => {
            alert("graph validation failed");
            console.log(err);
        });
    };

    const DisableGraphPlugin= () => {
        const newChoice=false;
        alert("Disabling Graph Plugin");
        console.log("Disabling Graph Plugin");
        setChildChoice(false);
        setchoice(false);
    };
    
    if(choice==false)
        {
            return(
                <div>
                    <button className="GraphPluginChoiceButton" onClick={EnableGraphPlugin}>Enable Graph Plugin</button>
                </div>
            );
        }
    else
    {
        return(
            <div>
                <button className="GraphPluginChoiceButton" onClick={DisableGraphPlugin}>Disable Graph plugin</button>
            </div>
        )
    }
    
}