import {useState} from "react";
import "./WebSearchPluginButton.css";

export default function WebSearchPluginButton({setchoice})
{
    const [choice, setChildChoice] = useState(false);

    const EnableWebSearch = () => {
        const newChoice=true;
        alert("Enabling Web Search");
        setChildChoice(true);
        setchoice(true);
    }

    const DisableWebSearch = () => {
        const newChoice=false;
        alert("Disabling Web Search");
        setChildChoice(false);
        setchoice(false);
    }
    
    if(choice==false)
        {
            return(
                <div>
                    <button onClick={EnableWebSearch} className="webButton" >Enable Web Search</button>
                </div>
            );
        }
    else
    {
        return(
            <div>
                <button onClick={DisableWebSearch} className = "webButton" >Disable Web Search</button>
            </div>
        )
    }
    
}
