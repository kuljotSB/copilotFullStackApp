import {useState} from "react";
import "./AISearchPluginButton.css";

export default function AISearchPluginButton({setchoice})
{
    const [choice, setChildChoice] = useState(false);

    const EnableAISearch = () => {
        const newChoice=true;
        alert("Enabling AI Search");
        console.log("Enabling AI Search");
        setChildChoice(true);
        setchoice(true);
    };

    const DisableAISearch= () => {
        const newChoice=false;
        alert("Disabling AI Search");
        console.log("Disabling AI Search");
        setChildChoice(false);
        setchoice(false);
    };
    
    if(choice==false)
        {
            return(
                <div>
                    <button className="AISearchPluginButton" onClick={EnableAISearch}>Enable AI Search</button>
                </div>
            );
        }
    else
    {
        return(
            <div>
                <button className="AISearchPluginButton" onClick={DisableAISearch}>Disable AI Search</button>
            </div>
        )
    }
    
}