using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePuzzle_2.Services.Models
{
    public static class LevelEditorHTML
    {
        public static readonly string HtmlText = @"
            
            <!doctype html>
                <html lang=""en"">

                <head>
                    <meta charset=""utf-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <style>
                        * {
                            margin: 0px;
                            padding: 0px;
                        }

                        #leftMenu {
                            float: left;
                            width: 30vw;
                            height: 95vh;
                            background-color: rgb(108, 51, 100);
                            margin: 5px, 0px;
                            padding: 15px, 0px;
                        }

                        #leftMenuContainer {
                            margin: auto;
                            padding: 10px;
                            height: 90%;
                            width: 90%;
                        }

                        h1 {
                            text-align: center;
                        }

                        .numberInput {
                            height: 25px;
                        }

                        .txt {
                            font-family: sans-serif;
                            color: white;
                        }

                        .row {
                            width: 100%;
                            height: auto;
                            display: flex;
                        }

                        .col-2 {
                            flex: 1;
                            width: 50%;
                            height: 100%;
                            padding: 5px;
                        }

                        .col-3 {
                            flex: 1;
                            width: 30%;
                            height: 100%;
                            padding: 5px;
                        }

                        .cardContainer {
                            padding: 5px;
                            width: 70px;
                            height: 70px;
                            text-align: center;
                            background-color: black;
                            margin: 5px;
                        }

                        .objectIcon {
                            height: 70%;
                            margin: 0;
                            padding: 0;
                            font-family: sans-serif;
                            font-size: xx-large;
                            white-space: pre;
                        }

                        .objectIcon span {
                            margin: 0;
                            padding: 0;
                        }

                        .objectLabel {
                            width: 100%;
                            height: 30%;
                            margin-top: 5px;
                            font-family: sans-serif, ""Helvetica Neue"", ""Lucida Grande"", Arial;
                            font-size: small;
                        }

                        #boardPanel {
                            width: 60vw;
                            height: 90vh;
                            margin-left: 5vw;
                            border: 3px solid rgb(108, 51, 100);
                            text-align: center;
                            align-items: center;
                            justify-content: center;
                            padding: 5px;
                            position: relative;
                            display: flex;
                        }

                        #grid {
                            width: 100%;
                            height: auto;
                            align-items: center;
                        }

                        .gridRow {
                            margin: auto;
                            text-align: center;
                            width: 95%;
                            display: flex;
                            justify-content: center;
                        }

                        .gridCol {
                            flex: 1;
                            border: 2px solid gray;
                            margin: 5px;
                            background-color: black;
                        }

                        .square-box {
                            position: relative;
                            width: 10%;
                            overflow: hidden;
                        }

                        .square-box:before {
                            content: """";
                            display: block;
                            padding-top: 100%;
                        }

                        .square-content {
                            position: absolute;
                            top: 0;
                            left: 0;
                            bottom: 0;
                            right: 0;
                            text-align: center;
                        }

                        .colsCenter {
                            margin: auto;
                            height: auto;
                            width: 100%;
                        }

                        .selectedCard {
                            border: solid green 4px;
                            transform: scale(1.2);
                            transition: transform .2s ease-in-out;
                            box-shadow: 0 0 20px white;
                        }

                        .filledCell {
                            line-height: 50px;
                        }

                        .btn {
                            border: none;
                            color: white;
                            padding: 16px 32px;
                            text-align: center;
                            text-decoration: none;
                            display: inline-block;
                            font-size: 16px;
                            margin: 4px 2px;
                            transition-duration: 0.4s;
                            cursor: pointer;
                            border-radius: 5px;
                        }

                        .btnGenerate {
                            background-color: rgb(61, 176, 61);
                        }

                        .btnGenerate:hover {
                            background-color: rgb(75, 224, 75);
                            color: white;
                        }

                        .btnReset {
                            background-color: rgb(231, 60, 60);
                        }

                        .btnReset:hover {
                            background-color: rgb(255, 100, 100);
                            color: white;
                        }

                        .configPanel {
                            width: 95%;
                            height: auto;
                            padding: 15px;
                        }

                        .configPanelContainer {
                            background-color: rgb(86, 43, 80);
                            padding: 5px;
                        }

                        .configInput {
                            width: 90%;
                        }
                    </style>

                    <title>Console Puzzle Level Editor</title>
                </head>

                <body>
                    <div id=""container"" style=""display: flex;"">
                        <div id=""leftMenu"">
                            <div id=""leftMenuContainer"">
                                <h1 class=""txt"">Level Editor</h1>
                                <div class=""row"">
                                    <div class=""col-2"">
                                        <label for=""boardWidth"" class=""txt"">Board Width</label>
                                        <input type=""number"" id=""boardWidth"" name=""boardWidth"" class=""numberInput"" value=""3"">
                                    </div>
                                    <div class=""col-2"">
                                        <label for=""boardWidth"" class=""txt"">Board Height</label>
                                        <input type=""number"" id=""boardHeight"" name=""boardHeight"" class=""numberInput"" value=""3"">
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div id=""card_0"" class=""cardContainer"" onclick=""activateCard(0)"">
                                        <div class=""objectIcon"" value=""(P)"" style=""color: magenta;"">P</div>
                                        <div class=""objectLabel txt"">Player</div>
                                    </div>
                                    <div id=""card_1"" class=""cardContainer"" onclick=""activateCard(1)"">
                                        <div class=""objectIcon"" value=""(p)"" style=""color: rgb(16, 18, 187);"">P</div>
                                        <div class=""objectLabel txt"">Player 2</div>
                                    </div>
                                    <div id=""card_2"" class=""cardContainer"" onclick=""activateCard(2)"">
                                        <div class=""objectIcon"" value=""(@)"" style=""color: green;"">@</div>
                                        <div class=""objectLabel txt"">Exit</div>
                                    </div>
                                    <div id=""card_3"" class=""cardContainer"" onclick=""activateCard(3)"">
                                        <div class=""objectIcon"" value=""[ ]"" style=""color: black; background-color: gray;"">[ ]</div>
                                        <div class=""objectLabel txt"">Wall</div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div id=""card_4"" class=""cardContainer"" onclick=""activateCard(4)"">
                                        <div class=""objectIcon"" value=""($)"" style=""color: rgb(245, 255, 151)"">$</div>
                                        <div class=""objectLabel txt"">Coin</div>
                                    </div>
                                    <div id=""card_5"" class=""cardContainer"" onclick=""activateCard(5)"">
                                        <style>
                                            .coinDoor:before {
                                                color: rgb(228, 176, 33);
                                                content: ""["";
                                            }

                                            .coinDoor:after {
                                                color: rgb(228, 176, 33);
                                                content: ""]"";
                                            }
                                        </style>
                                        <div class=""objectIcon coinDoor"" value=""[1]"" style=""color: rgb(245, 255, 151);"">1</div>
                                        <div class=""objectLabel txt"">Coin Door</div>
                                    </div>
                                    <div id=""card_6"" class=""cardContainer"" onclick=""activateCard(6)"">
                                        <div class=""objectIcon"" value=""(a)"" style=""color: rgb(118, 243, 255)"">a</div>
                                        <div class=""objectLabel txt"">Key</div>
                                    </div>
                                    <div id=""card_7"" class=""cardContainer"" onclick=""activateCard(7)"">
                                        <style>
                                            .keyDoor:before {
                                                color: rgb(6, 191, 210);
                                                content: ""["";
                                            }

                                            .keyDoor:after {
                                                color: rgb(6, 191, 210);
                                                content: ""]"";
                                            }
                                        </style>
                                        <div class=""objectIcon keyDoor"" value=""[a]"" style=""color: rgb(118, 243, 255);"">a</div>
                                        <div class=""objectLabel txt"">Key Door</div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div id=""card_8"" class=""cardContainer"" onclick=""activateCard(8)"">
                                        <div class=""objectIcon"" value=""{>}"" style=""color: white;"">{>}</div>
                                        <div class=""objectLabel txt"">One-Way</div>
                                    </div>
                                    <div id=""card_9"" class=""cardContainer"" onclick=""activateCard(9)"">
                                        <style>
                                            .portal:before {
                                                color: rgb(16, 18, 187);
                                                content: ""|"";
                                            }

                                            .portal:after {
                                                color: rgb(16, 18, 187);
                                                content: ""|"";
                                            }
                                        </style>
                                        <div class=""objectIcon portal"" value=""|a|"" style=""color: rgb(54, 104, 255);"">a</div>
                                        <div class=""objectLabel txt"">Portal One</div>
                                    </div>
                                    <div id=""card_10"" class=""cardContainer"" onclick=""activateCard(10)"">
                                        <div class=""objectIcon portal"" value=""|A|"" style=""color: rgb(54, 104, 255);"">A</div>
                                        <div class=""objectLabel txt"">Portal Two</div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div id=""card_11"" class=""cardContainer"" onclick=""activateCard(11)"">
                                        <div class=""objectIcon"" value=""(#)"" style=""color: rgb(228, 176, 33);"">#</div>
                                        <div class=""objectLabel txt"">Box</div>
                                    </div>
                                    <div id=""card_12"" class=""cardContainer"" onclick=""activateCard(12)"">
                                        <style>
                                            .pressBtn:before {
                                                color: white;
                                                content: ""("";
                                            }

                                            .pressBtn:after {
                                                color: white;
                                                content: "")"";
                                            }
                                        </style>
                                        <div class=""objectIcon pressBtn"" value=""(0)"" style=""color: white; background-color: gray;""> - </div>
                                        <div class=""objectLabel txt"">Press Btn</div>
                                    </div>
                                    <div id=""card_13"" class=""cardContainer"" onclick=""activateCard(13)"">
                                        <style>
                                            .btnDoor:before {
                                                color: rgb(6, 191, 210);
                                                content: ""["";
                                            }

                                            .btnDoor:after {
                                                color: rgb(6, 191, 210);
                                                content: ""]"";
                                            }
                                        </style>
                                        <div class=""objectIcon btnDoor"" value=""{0}"" style=""color: rgb(118, 243, 255);""> - </div>
                                        <div class=""objectLabel txt"">Btn Door</div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div id=""card_14"" class=""cardContainer"" onclick=""activateCard(14)"">
                                        <div class=""objectIcon"" value=""(X)"" style=""color: red;"">x</div>
                                        <div class=""objectLabel txt"">The Guard</div>
                                    </div>
                                    <div id=""card_15"" class=""cardContainer"" onclick=""activateCard(15)"">
                                        <div class=""objectIcon"" value=""=(]"" style=""color: red;"">=(]</div>
                                        <div class=""objectLabel txt"">Cannon</div>
                                    </div>
                                    <div id=""card_16"" class=""cardContainer"" onclick=""activateCard(16)"">
                                        <style>
                                            .seeker:before {
                                                color: red;
                                                background-color: red;
                                                content: ""||"";
                                            }

                                            .seeker:after {
                                                color: red;
                                                background-color: red;
                                                content: ""||"";
                                            }
                                        </style>
                                        <div class=""objectIcon seeker"" value=""|?|""
                                            style=""color: red; background-color: rgb(235, 54, 54);"">?</div>
                                        <div class=""objectLabel txt"">The Seeker</div>
                                    </div>
                                    <div id=""card_17"" class=""cardContainer"" onclick=""activateCard(17)"">
                                        <div class=""objectIcon"" value=""( )""> </div>
                                        <div class=""objectLabel txt"">Blank</div>
                                    </div>
                                </div>

                                <div id=""configPanel"" class=""configPanel"">
                                    <h2 class=""txt"">Object Configuration</h2>
                                    <div class=""row configPanelContainer"">
                                        <div class=""col-3"">
                                            <label for=""objNumValue"" class=""txt"">Number</label>
                                            <input class=""numberInput configInput"" type=""number"" id=""objNumValue"" name=""objNumValue""
                                                value=1 disabled min=0 default=1>
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""objCharValue"" class=""txt"">Letter</label>
                                            <input class=""numberInput configInput"" type=""text"" id=""objCharValue"" name=""objCharValue""
                                                onkeypress='return ((event.charCode >= 65 && event.charCode <= 90) || (event.charCode >= 97 && event.charCode <= 122) || (event.charCode == 32))'
                                                maxlength=""1"" value=""a"" disabled default=""a"">
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""dir"" class=""txt"">Direction</label>

                                            <select class=""numberInput configInput"" name=""dir"" id=""dir"" disabled default=""up"">
                                                <option value=""up"">Up</option>
                                                <option value=""right"">Right</option>
                                                <option value=""down"">Down</option>
                                                <option value=""left"">Left</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div class=""row configPanelContainer"">
                                        <div class=""col-3"">
                                            <label for=""enemySpeed"" class=""txt"">Speed</label>
                                            <input class=""numberInput configInput"" type=""number"" step=0.1 id=""enemySpeed""
                                                name=""enemySpeed"" min=0.1 value=1 disabled default=1>
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""enemyDelay"" class=""txt"">Delay</label>
                                            <input class=""numberInput configInput"" type=""number"" step=100 id=""enemyDelay""
                                                name=""enemyDelay"" min=100 value=500 disabled default=500>
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""enemyStartDelay"" class=""txt"">Start Delay</label>
                                            <input class=""numberInput configInput"" type=""number"" step=100 id=""enemyStartDelay""
                                                name=""enemyStartDelay"" min=0 value=0 disabled default=0>
                                        </div>
                                    </div>

                                    <div class=""row configPanelContainer"">
                                        <div class=""col-3"">
                                            <label for=""enemyBulletSpeed"" class=""txt"">Bullet Speed</label>
                                            <input class=""numberInput configInput"" type=""number"" step=0.1 id=""enemyBulletSpeed""
                                                name=""enemyBulletSpeed"" min=0.1 value=1 disabled default=1>
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""enemyRateOfFire"" class=""txt"">Rate of Fire</label>
                                            <input class=""numberInput configInput"" type=""number"" step=0.1 id=""enemyRateOfFire""
                                                name=""enemyRateOfFire"" min=0.1 value=1 disabled default=1>
                                        </div>
                                        <div class=""col-3"">
                                            <label for=""enemySightRadius"" class=""txt"">Sight Radius</label>
                                            <input class=""numberInput configInput"" type=""number"" step=1 id=""enemySightRadius""
                                                name=""enemySightRadius"" min=1 value=3 disabled default=3>
                                        </div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <button class=""btn btnGenerate"" onclick=""generateJson()"">Generate JSON</button>
                                    <button class=""btn btnReset"" onclick=""location.reload();"">Reset Grid</button>
                                </div>

                            </div>
                        </div>
                        <div id=""boardPanel"">
                            <div id=""grid"">
                                <div id=""row_1"" class=""gridRow"">
                                    <div class=""square-box"">
                                        <div id=""row_0_col_0"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_0_col_1"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_0_col_2"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                </div>
                                <div id=""row_1"" class=""gridRow"">
                                    <div class=""square-box"">
                                        <div id=""row_1_col_0"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_1_col_1"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_1_col_2"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                </div>
                                <div id=""row_1"" class=""gridRow"">
                                    <div class=""square-box"">
                                        <div id=""row_2_col_0"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_2_col_1"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                    <div class=""square-box"">
                                        <div id=""row_2_col_2"" class=""gridCol square-content"" onclick=""onCellClick(this)"">
                                            <div class=""objectIcon"" value=""( )""> </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <script>
                        let gridWidth = document.getElementById(""boardWidth"");
                        let gridHeight = document.getElementById(""boardHeight"");

                        gridWidth.addEventListener('change', OnGridChanged);
                        gridHeight.addEventListener('change', OnGridChanged);

                        let boardWidth = 3;
                        let boardHeight = 3;
                        let board = document.getElementById(""grid"");

                        let txtValue = document.getElementById(""objNumValue"");
                        let numValue = document.getElementById(""objCharValue"");
                        let dpdDir = document.getElementById(""dir"");

                        let numSpeed = document.getElementById(""enemySpeed"");
                        let numDelay = document.getElementById(""enemyDelay"");
                        let numStartDelay = document.getElementById(""enemyStartDelay"");

                        let numBulletSpeed = document.getElementById(""enemyBulletSpeed"");
                        let numRateOfFire = document.getElementById(""enemyRateOfFire"");
                        let numSightRadius = document.getElementById(""enemySightRadius"");

                        let configInputs = [txtValue, numValue, dpdDir, numSpeed, numDelay, numStartDelay, numBulletSpeed, numRateOfFire, numSightRadius];

                        let enemiesConfig = [];

                        let enemiesValues = [""(X)"", ""/\'\\"", ""[)="", ""\\./"", ""=(]"", ""|?|""];

                        function AlphaOnly(event) {
                            var key = event.keyCode;
                            return ((key >= 65 && key <= 90) || key == 8);
                        };

                        function OnGridChanged(e) {
                            let value = e.srcElement.value;
                            let input = e.srcElement.id;

                            if (input == ""boardWidth"") {
                                boardWidth = value;
                            }
                            else {
                                boardHeight = value;
                            }
                            UpdateBoard();
                        }

                        function UpdateBoard() {
                            board.innerHTML = """";
                            for (let y = 0; y < boardHeight; y++) {
                                let row = document.createElement('div');
                                row.setAttribute('id', 'row_' + y);
                                row.setAttribute('class', 'gridRow');
                                board.appendChild
                                for (let x = 0; x < boardWidth; x++) {
                                    let box = document.createElement('div');
                                    let col = document.createElement('div');
                                    let blankIcon = document.createElement('div');
                                    blankIcon.setAttribute('class', 'objectIcon')
                                    blankIcon.setAttribute('value', '( )')
                                    col.setAttribute('id', 'row_' + y + '_col_' + x);
                                    col.setAttribute('class', 'gridCol square-content');
                                    col.setAttribute('onclick', 'onCellClick(this)')
                                    box.setAttribute('class', 'square-box')
                                    col.appendChild(blankIcon);
                                    box.appendChild(col);
                                    row.appendChild(box);
                                }
                                board.appendChild(row);
                            }
                            enemiesConfig = [];
                        }

                        function activateCard(value) {
                            let oldCard = document.getElementsByClassName(""selectedCard"")[0];
                            let newCard = document.getElementById('card_' + value);
                            if (oldCard != null && oldCard != undefined) {
                                oldCard.classList.remove(""selectedCard"")
                                if (oldCard.id == newCard.id) {
                                    DisableAllConfig();
                                    return;
                                }
                            }
                            DisableAllConfig();
                            let inputs = MapEnabledInputs(value);
                            configInputs.forEach(item => {
                                if (inputs.includes(configInputs.indexOf(item))) {
                                    item.disabled = false;
                                    item.setAttribute('required', true);
                                }
                            });
                            newCard.classList.add(""selectedCard"");
                        }

                        function onCellClick(sender) {
                            let row = parseInt(sender.id.substring(4, sender.id.indexOf('col_') - 1));
                            let col = parseInt(sender.id.substring(sender.id.indexOf('col_') + 4));
                            let oldCellValue = document.getElementById(""grid"").children[row].children[col].children[0].getElementsByClassName(""objectIcon"")[0].getAttribute('value');
                            if(enemiesValues.includes(oldCellValue)){
                                enemiesConfig = enemiesConfig.filter(function( obj ) {
                                    return obj.x != col || obj.y != row;
                                });
                            }

                            let card = document.getElementsByClassName(""selectedCard"")[0];
                            let cardNumber = parseInt(card.id.substring(5));
                            if (card != null && card != undefined) {
                                let oldInner = sender.innerHTML;
                                sender.innerHTML = card.innerHTML;
                                sender.removeChild(sender.getElementsByClassName(""objectLabel"")[0]);
                                let icon = sender.getElementsByClassName(""objectIcon"")[0];
                                let result = MapValues(icon, cardNumber, col, row);

                                if(hasNullOrEmpty(enemiesConfig)){
                                    alert(""Invalid enemy configuration. Please fill all required fields with a valid value."");
                                    enemiesConfig.pop();
                                    sender.innerHTML = oldInner;
                                    return;
                                }
                                icon.setAttribute('value', result[1]);
                                icon.innerHTML = result[0];
                                icon.style.height = '100%'
                                sender.classList.add('filledCell');
                            }
                        }

                        function DisableAllConfig() {
                            configInputs.forEach(item => {
                                item.disabled = true;
                                item.value = item.getAttribute('default');
                                item.removeAttribute('required');
                            });
                        }

                        function isNullOrEmpty(value){
                            return value == null || value == '' || value == undefined || value == NaN;
                        }

                        function hasNullOrEmpty(values){
                            let result = false;
                            values.forEach(item => {
                                if(isNullOrEmpty(item)){
                                    result = true;
                                }
                            });
                            return result;
                        }

                        function MapEnabledInputs(value) {
                            let inputs_to_enable = [];
                            switch (value) {
                                case 5:
                                case 12:
                                case 13:
                                    inputs_to_enable.push(0);
                                    break;
                                case 6:
                                case 7:
                                case 9:
                                case 10:
                                    inputs_to_enable.push(1);
                                    break;
                                case 8:
                                    inputs_to_enable.push(2);
                                    break;
                                case 14:
                                    inputs_to_enable.push(2, 3, 4, 5);
                                    break;
                                case 15:
                                    inputs_to_enable.push(2, 5, 6, 7);
                                    break;
                                case 16:
                                    inputs_to_enable.push(3, 8);
                                    break;
                                default:
                                    DisableAllConfig()
                            }
                            return inputs_to_enable;
                        }

                        function MapValues(icon, number, x = 0, y = 0) {
                            let result = [];
                            switch (number) {
                                case 5:
                                case 12:
                                case 13:
                                    result[0] = configInputs[0].value;
                                    let val1 = icon.getAttribute('value');
                                    result[1] = val1[0] + result[0] + val1[2];
                                    break;
                                case 6:
                                case 7:
                                case 9:
                                case 10:
                                    result[0] = configInputs[1].value;

                                    if (number == 9)
                                        result[0] = result[0].toLowerCase();
                                    if (number == 10)
                                        result[0] = result[0].toUpperCase();
                                    let val2 = icon.getAttribute('value');
                                    result[1] = val2[0] + result[0] + val2[2];
                                    break;
                                case 8:
                                    result[0] = configInputs[2].value;
                                    switch (result[0]) {
                                        case ""up"":
                                            result[1] = ""{↑}"";
                                            result[0] = ""{↑}"";
                                            break;
                                        case ""right"":
                                            result[1] = ""{>}"";
                                            result[0] = ""{>}"";
                                            break;
                                        case ""down"":
                                            result[1] = ""{↓}"";
                                            result[0] = ""{↓}"";
                                            break;
                                        case ""left"":
                                            result[1] = ""{<}"";
                                            result[0] = ""{<}"";
                                            break;
                                    }

                                    break;
                                case 14:
                                    let val3 = icon.getAttribute('value');
                                    result[1] = val3;
                                    result[0] = val3;
                                    if(hasNullOrEmpty([configInputs[2].value, configInputs[3].value, configInputs[4].value, configInputs[5].value])){
                                        enemiesConfig.push(undefined);
                                        break;
                                    }
                                    let enemyConfig1 = new enemyData({
                                        x: x, y: y,
                                        dir: configInputs[2].value,
                                        speed: parseFloat(configInputs[3].value),
                                        delay: parseInt(configInputs[4].value),
                                        startDelay: parseInt(configInputs[5].value)
                                    });
                                    enemiesConfig.push(enemyConfig1);
                                    break;
                                case 15:
                                    let val4 = icon.getAttribute('value');
                                    result[1] = val4;
                                    result[0] = val4;
                                    switch (configInputs[2].value) {
                                        case ""up"":
                                            result[1] = ""/\'\\"";
                                            result[0] = ""/\'\\"";
                                            break;
                                        case ""right"":
                                            result[1] = ""[)="";
                                            result[0] = ""[)="";
                                            break;
                                        case ""down"":
                                            result[1] = ""\\./"";
                                            result[0] = ""\\./"";
                                            break;
                                        case ""left"":
                                            result[1] = ""=(]"";
                                            result[0] = ""=(]"";
                                            break;
                                    }
                                    if(hasNullOrEmpty([configInputs[2].value, configInputs[5].value, configInputs[6].value, configInputs[7].value])){
                                        enemiesConfig.push(undefined);
                                        break;
                                    }
                                    let enemyConfig2 = new enemyData({
                                        x: x, y: y,
                                        dir: configInputs[2].value,
                                        startDelay: parseInt(configInputs[5].value),
                                        bulletSpeed: parseFloat(configInputs[6].value),
                                        rateOfFire: parseFloat(configInputs[7].value)
                                    });
                                    enemiesConfig.push(enemyConfig2);
                                    break;
                                case 16:
                                    let val5 = icon.getAttribute('value');
                                    result[1] = val5;
                                    result[0] = val5;

                                    if(hasNullOrEmpty([configInputs[3].value, configInputs[8].value])){
                                        enemiesConfig.push(undefined);
                                        break;
                                    }
                                    let enemyConfig3 = new enemyData({
                                        x: x, y: y,
                                        speed: parseFloat(configInputs[3].value),
                                        sightRadius: parseInt(configInputs[8].value)
                                    });
                                    enemiesConfig.push(enemyConfig3);
                                    break;
                                default:
                                    let valx = icon.getAttribute('value');
                                    result[1] = valx;
                                    result[0] = valx;
                                    break;
                            }
                            return result;
                        }

                        class enemyData {
                            constructor({ x, y, speed = 0, delay = 0, dir = ""left"", startDelay = 0, bulletSpeed = 0, rateOfFire = 0, sightRadius = 0 } = {}) {
                                this.x = x;
                                this.y = y;
                                this.speed = speed;
                                this.delay = delay;
                                this.dir = dir;
                                this.startDelay = startDelay;
                                this.bulletSpeed = bulletSpeed;
                                this.rateOfFire = rateOfFire;
                                this.sightRadius = sightRadius;
                            }
                        }

                        class BoardObject {
                            constructor() {
                                this.mapping = [];
                                this.config = {
                                    enemiesData: []
                                }
                            }
                        }

                        function generateJson() {
                            if(hasNullOrEmpty(enemiesConfig)){
                                alert(""There's an invalid enemy configuration."");
                                return;
                            }
                            let data = new BoardObject();

                            let rows = board.children;
                            for (let y = 0; y < rows.length; y++) {
                                let colsBox = rows[y].children;
                                let rowString = """";
                                for (let x = 0; x < colsBox.length; x++) {
                                    let icon = colsBox[x].children[0].getElementsByClassName(""objectIcon"")[0];
                                    rowString += icon.getAttribute('value') + ""&"";
                                }
                                data.mapping.push(rowString.slice(0, -1));
                            }
                            data.config.enemiesData = enemiesConfig;
                            let json = JSON.stringify(data, undefined, 2).replaceAll('\""','\'');
                            console.log(json);
                            copyToClipboard(json);
                            alert(""Json copied to clipboard!"");
                        }

                        function copyToClipboard(json) {
                            document.addEventListener('copy', (e) => CopyListener(e, json));
                            document.execCommand('copy');
                        };

                        function CopyListener(e, json) {
                            e.clipboardData.setData('text/plain', json);
                            e.preventDefault();
                            document.removeEventListener('copy', CopyListener);
                        }
                    </script>

                </body>

                </html>
        ";
    }
}
