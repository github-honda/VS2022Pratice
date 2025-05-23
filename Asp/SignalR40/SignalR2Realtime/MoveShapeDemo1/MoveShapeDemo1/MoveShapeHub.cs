﻿using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoveShapeDemo1
{
    public class MoveShapeHub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);
        }
        public class ShapeModel
        {
            // We declare Left and Top as lowercase with 
            // JsonProperty to sync the client and server models
            [JsonProperty("left")]
            public double Left { get; set; }
            [JsonProperty("top")]
            public double Top { get; set; }
            // We don't want the client to get the "LastUpdatedBy" property
            [JsonIgnore]
            public string LastUpdatedBy { get; set; }
        }
    }
}