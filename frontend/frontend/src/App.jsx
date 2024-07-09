import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import InputTextBox from './InputTextBox'
import RenderMessage from './RenderMessage'
import RenderPlanner from "./RenderPlanner";
import WebSearchPluginButton from './WebSearchPluginButton'
import AISearchPluginButton from './AISearchPluginButton'
import GraphPluginButton from './GraphPluginButton'
import CopilotIconComponent from './CopilotIconComponent'
import ChatResponseHeader from './ChatResponseHeader'
import SemanticKernelLogoComponent from './SemanticKernelLogoComponent'
import SemanticKernelHeader from './SemanticKernelHeader'


function App() {
  const [chatConvo, setChatConvo]= useState([{userQuery: "", chatResponse: "", plannerMessage:""}]);
  const [webSearch, webSearchChoice] = useState(false);
  const [AISearchPluginButtonChoice, setAISearchPluginButtonChoice] = useState(false);
  const [GraphPluginButtonChoice, setGraphPluginButtonChoice] = useState(false);
  
  return(
    <div className='container-header'>
     <div className = "chatResponseComponent">
    <RenderMessage chatConvo={chatConvo} />
     
    </div>
    <div className = "plannerComponent">
    <RenderPlanner chatConvo={chatConvo} className="RenderPlanner" />
    </div>
   
    
    <div className = "inputComponent">
    <InputTextBox setChatConvo={setChatConvo} webSearchChoice={webSearch} AISearchChoice={AISearchPluginButtonChoice} graphPluginChoice={GraphPluginButtonChoice} />
    </div>
    <div className="WebSearchComponent">
    <WebSearchPluginButton className="WebSearchPluginButton" setchoice={webSearchChoice} />
  
    </div>
    <div className="AISearchComponent">
    <AISearchPluginButton  setchoice={setAISearchPluginButtonChoice} />
   </div>

   <div className="GraphPluginComponent">
    <GraphPluginButton setchoice={setGraphPluginButtonChoice} />
    </div>

    <div className = "CopilotIconDiv">
      <CopilotIconComponent />
    </div>

    <div className = "ChatResponseHeaderDiv">
      <ChatResponseHeader />
      </div>

    <div className = "SemanticKernelLogoComponentDiv">
    <SemanticKernelLogoComponent />
    </div>

    <div className = "SemanticKernelHeaderDiv">
      <SemanticKernelHeader />
    </div>
    
    </div>
  );
}
export default App
