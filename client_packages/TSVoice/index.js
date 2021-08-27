var tsBrowser = null;
var refresh = 0;
var browserLoad = false;
var canBeRefreshed = true;
var currentUrl = '';
let ringtone = null;
var pushtotalk = true;
let LocalPlayer = mp.players.local;

mp.events.add('ConnectTeamspeak', (status) => {
    if (status) {
        tsBrowser = mp.browsers.new('');
        setTimeout(function () {
            refresh = 1;
            //mp.game.audio.playSoundFrontend(-1, "Hack_Success", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
			mp.events.callRemote('Server:Voice:SetRange');
        }, 500);
    } else {
        refresh = 0;
        if (tsBrowser != null) {
            tsBrowser.destroy();
            tsBrowser = null;
        }
        //mp.game.audio.playSoundFrontend(-1, "Hack_Failed", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
    }
});

function distanceCalc(vector1, vector2) {
    return mp.game.system.vdist2(vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z);
}

function subtract(vector1, vector2) {
    vector1.x = vector1.x - vector2.x;
    vector1.y = vector1.y - vector2.y;
    vector1.z = vector1.z - vector2.z;
    return vector1;
}

var isFunking = false;

mp.events.add('render', () => {
    if(mp.game.controls.isControlPressed(0, 173) && mp.players.local.getVariable('Funk_Frequenz') != 0) {
        if(pushtotalk && !isFunking) {
            isFunking = true;
            if(!mp.players.local.vehicle) {
                mp.events.callRemote("Funk:Talk");
            } else {
                mp.events.callRemote("Funk:Talk2");
            }
        }
    } else {
        if(pushtotalk && isFunking) {
            isFunking = false;
            if(!mp.players.local.vehicle) {
                mp.events.callRemote("Funk:NoTalk");
            } else {
                mp.events.callRemote("Funk:NoTalk2");
            }
        }
    }

    if (tsBrowser != null && canBeRefreshed) {
        if (refresh == 1) {
            canBeRefreshed = false;
            var player = mp.players.local;
            var playerPos = player.position;
            var playerRot = player.getHeading();
            var rotation = Math.PI / 180 * (playerRot * -1);
            var playerNames = new Array();

            var callingPlayerName = player.getVariable("CALLING_PLAYER_NAME");
            if (callingPlayerName && player.getVariable("CALL_IS_STARTED")) {
                if (callingPlayerName != "") {
                    playerNames.push(callingPlayerName + "~10~0~0~3");
                }
            }

			mp.players.forEach(
				(target, id) => {
                    let playerDead = target.getVariable('InDeath') === true;
                    if (! playerDead) {
                        if(target.getVariable('Funk_Frequenz') == player.getVariable('Funk_Frequenz')) {
                            if(target.getVariable('Funk_Talk') == true) {
                                playerNames.push(target.name + "~10~0~0~3");
                            }
                        }
                    }
				}
			);

			mp.players.forEach(
				(target, id) => {
                    let playerDead = target.getVariable('InDeath') === true;
                    if (! playerDead) {
                        if(target.getVariable('Handy_ID') == player.getVariable('Handy_ID') && player.getVariable('Handy_ID') !== null && player.getVariable('Handy_ID') !== undefined) {
                            if(target.getVariable('IsHeCalling') == true || player.getVariable('IsHeCalling') == true) {
                                playerNames.push(target.name + "~10~0~0~3");
                            }
                        }
                    }
				}
			);

			
            mp.players.forEachInStreamRange(
                (playerStreamed, id) => {
					if (playerStreamed !== mp.players.local) {
                        let playerDead = playerStreamed.getVariable('InDeath') === true;
						var streamedPlayerPos = playerStreamed.position;
						var distance = distanceCalc(playerPos, streamedPlayerPos);
						var voiceRange = playerStreamed.getVariable("CLIENT_RANGE");
						var volumeModifier = 0;
						var range = 20;
						if (voiceRange == "Weit") {
							range = 200;
						}
						if (voiceRange == "Normal") {
							range = 20;
						}
						if (voiceRange == "Leise") {
							range = 5;
						}
						else if (voiceRange == "Stumm") {
							range = 0;
						}
						if (distance > 55) {
							volumeModifier = (distance * -5 / 10);
						}
						if (volumeModifier > 0) {
							volumeModifier = 0;
						}

						if (distance < range && ! playerDead) {
				
							var subPos = subtract(streamedPlayerPos, playerPos);
							var x = subPos.x * Math.cos(rotation) - subPos.y * Math.sin(rotation);
							var y = subPos.x * Math.sin(rotation) + subPos.y * Math.cos(rotation);
							x = x * 7 / range;
							y = y * 7 / range;
							var isDeath = playerStreamed.isDead();
							if (isDeath == null || !isDeath) {
								playerNames.push(playerStreamed.name + "~" + (Math.round(x * 1000) / 1000) + "~" + (Math.round(y * 1000) / 1000) + "~0~" + (Math.round(volumeModifier * 1000) / 1000));
							}
						}
					}
                }
            );
			

			
				
            var newUrl = "http://localhost:15555/players/" + player.name + "/" + playerNames.join(";") + "/";
            if (currentUrl != newUrl) {
                tsBrowser.url = newUrl;
                currentUrl = newUrl;
            }
            setTimeout(function () {
                canBeRefreshed = true;
            }, 500);
        }
    }
});

let currentRange = mp.players.local.getVariable('CLIENT_RANGE');
mp.keys.bind(0x59, true, function() {
    mp.events.callRemote('Server:Voice:SwitchRange');
    currentRange = mp.players.local.getVariable('CLIENT_RANGE')
});

let micRangeTextEnabled = true;
mp.events.add('render', () => {
    if (micRangeTextEnabled) {
        if(mp.players.local.getVariable('CLIENT_RANGE') == "Leise")
            mp.events.call("overlay:changeimage", "voice", "user2.png");

        if(mp.players.local.getVariable('CLIENT_RANGE') == "Normal")
            mp.events.call("overlay:changeimage", "voice", "user3.png");

        if(mp.players.local.getVariable('CLIENT_RANGE') == "Weit")
            mp.events.call("overlay:changeimage", "voice", "user4.png");
    }
});

mp.events.add('Funk:ChangePushtotalk', (state) => {
    pushtotalk = state;

    if(!pushtotalk) {
        //mp.events.call("sendPlayerNotification", "Du hast deinen Voice-Status auf Dauersenden geändert.", 5000, "green", "INFORMATION", "");
    }

    if(pushtotalk) {
        //mp.events.call("sendPlayerNotification", "Du hast deinen Voice-Status auf Push-To-Talk geändert.", 5000, "green", "INFORMATION", "");
    }
});

function refreshHide() {

    if(!pushtotalk)
        mp.events.callRemote("Funk:ChangeTalkState", true);

    if(tsBrowser != null)
        tsBrowser.execute("document.getElementsByTagName('BODY')[0].style.display = 'none';");

    setTimeout(() => {
        refreshHide();
    }, 200);
}

refreshHide();