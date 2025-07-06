import logo from './logo.svg';
import './App.css';
import { Component } from 'react';

class App extends Component
{
  constructor ()
  {
    super();
    this.state = 
    {
      images: []
    }
  }

  getImages = async () => 
  {
    var response = await fetch (
      "api/images",
      {
      method: "get"
    }
    )

    var responsejson = await response.json();
    this.setState({
      images:responsejson
    })
  }
    render()
        {
          const images = this.state.images.map((item, index) => <li key={index}>{item.name}</li>)
          return(
              <div className='App'>
              <button onClick={this.getImages}> Load Images</button>   
              <ul>{images}</ul>
              </div>
            );
        }
}

export default App;
