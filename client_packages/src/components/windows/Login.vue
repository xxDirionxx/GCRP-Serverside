<!--TODO: add loading spinner-->
<template>
    <div class="login_window_gvmp">
        <div class="login_window_left"><img :src="require ('@/assets/gvmp_login_logo.png')"></div>
        <div class="login_window_right">
            <div v-if="kickmode == false && wrongmode == false">
                <div class="login_welcome">Willkommen {{ player.name.replace("_", " ") }} </div>
                <div class="login_password">
                    <input v-model="password" v-on:keyup.enter="login" placeholder="Passwort" type="password" autofocus></div>
                <div class="login_button" :disabled="password.length == 0" v-on:click="login" >Login</div>
                <div class="login_info"><span style ="color:red;"> {{ message }} </span><br>
                    <span v-if="seconds >= 10 && minutes >= 10" style="display: block;">Verbleibende Zeit zum Einloggen: {{ minutes }}:{{ seconds }}</span>
                    <span v-else-if="seconds < 10 && minutes >= 10" style="display: block;">Verbleibende Zeit zum Einloggen: {{ minutes }}:0{{ seconds }}</span>
                    <span v-else-if="seconds >= 10 && minutes < 10" style="display: block;">Verbleibende Zeit zum Einloggen: 0{{ minutes }}:{{ seconds }}</span>
                    <span v-else-if="seconds < 10 && minutes < 10" style="display: block;">Verbleibende Zeit zum Einloggen: 0{{ minutes }}:0{{ seconds }} </span>
                </div>
            </div>

            <div v-else-if="kickmode == true" style= "color:#fff;padding-top: 13vh; text-align:left; padding-left:1vh; padding-right:1vh;">
                <span style= "font-weight: 800"> Du hast zu lange gebraucht !</span><hr>
                Aufgrund das du längere Zeit am Login Fenster warst, ohne dich einzuloggen wurdest du vom Server gekickt.
                <br>Um wieder auf die Insel einzureisen musst du GTA neu starten und dich neu Whitelisten.
            </div>
            <div v-else-if="wrongmode == true" style= "color:#fff;padding-top: 13vh; text-align:left; padding-left:1vh; padding-right:1vh;">
                <span style= "font-weight: 800"> Sicherheitskick </span> <hr>
                Aufgrund das du mehrmals dein Passwort falsch eingegeben hast, wurdest du zur Sicherheit vom Server gekickt. <br>
                Um wieder auf die Insel einzureisen musst du GTA neu starten und dich neu Whitelisten.
            </div>
        </div>
        <div class="clear"></div>
    </div>
</template>

<script>
    import Windows from "../windows"
    var sha256 = require('js-sha256')
    var timer;
    export default Windows.register({
        name: "Login",
        props: {
            data: String
        },
        data() {
            return {
                player: JSON.parse(this.data),
                password: "",
                message: "",
                kickmode: false,
                wrongmode: false,
                seconds: 0,
                minutes: 3,
                try: 0
            }
        },
        methods: {
            login() {
                if (this.password == "") return
                this.triggerServer("PlayerLogin", sha256(this.password))
            },
            status(status) {
                if (status != "successfully") {
                    this.try++
                    this.message = status
                    if(this.try == 3){
                        this.wrongmode = true
                    }
                } else {
                    clearInterval(timer)
                    this.dismiss()
                }
            },
            kick(){
                this.kickmode = true
                this.triggerServer("kick")
            },
            time(){
                this.seconds--
                if(this.seconds < 0){
                    this.minutes--
                    if(this.minutes < 0){
                        this.kick()
                    }
                    else{
                        this.seconds = 59
                    }
                }
            }
        },
        mounted() {
            if(this.player.rank == 8 || this.player.rank == 11 || this.player.rank == 5 || this.player.rank == 6 || this.player.rank == 21 || this.player.rank == 4){
                this.minutes = 60
                this.seconds = 0
                this.trigger('rank',this.player.rank)
            }
            timer = setInterval(this.time,1000)
        }
    })
</script>

<style lang="scss" scoped>
    .clear {
        clear: both; }

    .login_window_gvmp {
        padding-top: 2vh;
        opacity: 0.99;
        position: absolute;
        width: 50vw;
        height: 40vh;
        left: 1VW;
        top: 50%;
        transform: translateY(-50%);
        margin-left: auto;
        margin-right: auto;
        left: 25%;
        transform: translateX(-50%, 0); 
    }
    .login_window_gvmp .login_window_left {
        width: 40%;
        height: 100%;
        float: left;
        box-shadow: -1vh 0 0px 0px rgba(218, 114, 0, 0.8);
        background-image: url("../../assets/logo_login_window.jpg");
        background-size: cover;
        transform-origin: bottom left;
        text-align: center;
        line-height:50vh;}
    .login_window_gvmp .login_window_left img {
        width:80%;
        height:auto;
    }
    .login_window_gvmp .changelog_button {
        position: absolute;
        right: 0.5em;
        font-size: 0.8vw;
        top: 0.5em;
        opacity: 0.5; }
    .login_window_gvmp .login_window_right {
        width: 60%;
        height: 100%;
        float: right;
        text-align: center;         
        background-image: linear-gradient(rgba(0,0,0,0.3), rgba(0,0,0,0.8));
        transform-origin: bottom left; 
        box-shadow: 1vh 0px 0px 0px rgba(218, 114, 0, 0.9);
    }
    .login_window_gvmp .login_window_right .login_welcome {
        margin-top: 2em;
        font-size: 2.5vh;
        margin-bottom: 2em;
        color: #FFF;
    }
    .login_window_gvmp .login_window_right .login_password input {
        height: 7vh;
        width: 60%;
        padding: 0.5em;
        font-size: 1.1vw;
        border: 0;
        border-radius: 0.3em;
        border: 0.1em solid #ccc;
        margin-bottom: 1em;
        color: #ccc; 
    }
    .login_window_gvmp .login_window_right .login_password .input_title {
        background: #464646;
        padding: 0.5em;
        border-radius: 0.3em;
        font-size: 0.5vw;
        position: absolute;
        margin-top: -1em;
        margin-left: 0.5em;
        text-transform: uppercase;
        box-shadow: 0px 0px 24px 0px rgba(0, 0, 0, 0.25);
        color: #fff; 
    }
    .login_window_gvmp .login_window_right .login_button {
        width: 60%;
        height: 3em;
        color: #fff;
        border-radius: 0.3em;
        margin: 0 auto;
        font-size: 0.8vw;
        line-height: 3em;
        background-image: linear-gradient(#fd9600, #ca8217);
        box-shadow: 0px 0px 24px 0px rgba(0, 0, 0, 0.25); }
    .login_window_gvmp .login_window_right .login_button:hover {
        background-image: linear-gradient(#fd9600, #e68b07);
        cursor: pointer; }
    .login_window_gvmp .login_window_right .login_info {
        font-size: 0.8vw;
        opacity: 0.8;
        text-shadow: 0px 0px 4px #80573c;
        color: #ccc; 
    }
</style>