<template>
    <div class="phone" v-bind:style="{display: visible}">
        <div class="device-s10">
            <div class="overflow">
                <div class="shadow"></div>
            </div>
            <div class="camera">
                <div class="linse"></div>
            </div>
            <div class="speaker"></div>
            <div class="powerbtn"></div>
            <div class="volumebtn"></div>
            <div class="addbtn"></div>
            <div v-on:click="homebutton()" class="mainmenu"></div>
            <div class="screen" v-bind:style="{display: visibleNormalScreen}">
                <component :is="appnormal" :data="datanormal"></component>
            </div>
            <div class="screen" v-bind:style="{display: visiblePhoneScreen}">
                <component :is="appphone" :data="dataphone"></component>
            </div>
        </div>
    </div>
</template>

<script>
import PhoneMainScreen from "../apps/PhoneMainScreen"
import CallManageApp from "../apps/CallManageApp"
import Components from "../components"
export default Components.register({
    name: "Smartphone",
    components: {
        PhoneMainScreen,
        CallManageApp
    },
    data() {
        return {
            appnormal: "",
            datanormal: {},
            appphone: "",
            dataphone: {},
            isWidescreen: false,
            time: "",
            visible: 'none',
            visibleNormalScreen: 'none',
            visiblePhoneScreen: 'none',
            beforevisible: false
        }
    },
    methods: {
        show: function(app, data = null) {
            if(this.visibleNormalScreen == 'none' && this.visiblePhoneScreen == 'none'){
                if(app == "CallManageApp"){
                    this.visiblePhoneScreen = 'block'
                    this.dataphone = data
                    this.appphone = app
                }else{
                    this.visibleNormalScreen = 'block'
                    this.datanormal = data
                    this.appnormal = app
                    this.visible = 'block'
                }
            }else{
                if(app == "CallManageApp"){
                    if(this.visible == 'block'){
                        this.beforevisible = true
                    }
                    this.dataphone = data
                    this.appphone = app
                    this.visibleNormalScreen = 'none'
                    this.visiblePhoneScreen = 'block'
                }else{
                    this.appnormal = app
                    this.datanormal = data
                    this.visible = 'block'
                }
            }
        },
        invisible(){
            if(this.visiblePhoneScreen != 'block'){
                this.beforevisible = false
            }
            this.visible = 'none'
        },
        change(){
            if(this.visiblePhoneScreen == 'block' || this.visible == 'none'){
                this.visiblePhoneScreen = 'none'
                this.appphone = ""
                this.dataphone = {}
                if(this.beforevisible == false || this.visible == 'none'){
                    this.visible = 'none'
                    this.trigger("cleanSmartphone",[])
                }else{
                    this.visibleNormalScreen = 'block'
                    this.trigger("activateCurser",[])
                    this.beforevisible = false
                }
            }else if(this.visibleNormalScreen == 'block'){
                this.appphone = ""
                this.dataphone = {}
                this.beforevisible = false
            }
        },
        refreshSmartphone(){
            this.visible = 'none'
            this.visiblePhoneScreen = 'none'
            this.visibleNormalScreen = 'none'
        },
        showCallScreen(){
            this.visiblePhoneScreen = 'block'
            this.visibleNormalScreen = 'none'
        },
        homebutton(){
            console.log("homescreen")
            if(this.visibleNormalScreen == 'block' && this.visible == 'block'){
                this.trigger("getHomeScreen",[])
            }else if(this.visiblePhoneScreen == 'block' && this.visible == 'block'){
                this.trigger("getHomeScreenCall",[])
                this.visiblePhoneScreen = 'none'
                this.visibleNormalScreen = 'block'
            }
        },
        onKeyUp: function(event){
            if(event.key == "n" && this.visiblePhoneScreen == 'block' && this.visibleNormalScreen == 'none' && this.visible == 'block'){
                this.trigger("callDeclined",[])
            }else if(event.key == "j" && this.visiblePhoneScreen == 'block' && this.visibleNormalScreen == 'none' && this.visible == 'block'){
                this.trigger("callAccepted",[])
            }else if(event.key == "m" && this.visiblePhoneScreen == 'block' && this.visibleNormalScreen == 'none' && this.visible == 'block'){
                this.trigger("micmute",[])
            }else if(event.key == "ArrowUp"){
                this.homebutton()
            }
        }
    },
    mounted() {
        window.addEventListener('keyup', this.onKeyUp)
    },
    destroyed() {
        window.removeEventListener("keyup", this.onKeyUp)
    }
})
</script>

<style lang="scss" scoped>
.smartphone {
    .page {
        height: inherit;
        width: inherit;
        position: relative;
    }
    .md-screen {
        z-index: 1;
        overflow-y: auto;
    }
}
.homebutton{     
    height: 2.25vh;
    width: 5.25vh;
    left: 50%;
    position: absolute;
    top: 48.2vh;
    margin-left: -2.625vh;
    background: #171818;
    z-index: 1;
    border-radius: 0.75vh;
}
.device-s10{
    width: 25vh;
    height: 50vh;
    background-color: #000000;
    border-radius: 2vh;

    margin-right: 1vh;
    margin-bottom: 1vh;
    border-color: #A9A9A9;
    border-style: solid;
    border-width: 0.27vh;
}
.device-s10 .screen{
    width: 24vh;
    margin-left: 0.25vh;
    position: relative;
    height: 47.75vh;
    margin-top: 0.75vh;
    z-index: 3;
    background: white;
    overflow: hidden;
    border-radius: 1.3125vh;
    box-shadow: none;
}
.device-s10 .overflow{
    width: 25vh;
    height: 50vh;
    position: absolute;
    border-radius: 2vh;
    overflow: hidden;
}

.device-s10 .overflow .shadow{
    box-shadow: inset 0 0 1vh 0 white, inset 0 0 1vh 0 rgba(255, 255, 255, 0.01), 0 0 1vh 0 white, 0 0 0.25vh 0 rgba(255, 255, 255, 0.05);
    height: 50vh;
    position: absolute;
    content: '';
    width: 23.7vh;
    border-radius: 2vh;
    z-index: 5;
    left: 0.4vh;
    pointer-events: none;
}

.device-s10 .time{
    left: 1.5vh;
    position: absolute;
    top: 1.9vh;
    z-index: 4;
    font-size: 1.5vh;
    color: #ffffff;
}

.device-s10 .camera{
    height: 1.75vh;
    width: 1.75vh;
    margin-left: 21.5vh;
    position: absolute;
    margin-top: 1.8vh;
    background: #000000;
    z-index: 10;
    border-radius: 100%;
}

.device-s10 .camera .linse{
    height: 0.75vh;
    width: 0.75vh;
    margin-left: 0.5vh;
    position: absolute;
    margin-top: 0.5vh;
    background: #303030;
    z-index: 10;
    border-radius: 100%;
}

.device-s10 .speaker{
    height: 0.4vh;
    width: 5vh;
    margin-left: 10vh;
    position: absolute;
    background: #303030;
    z-index: 1;
    border-bottom-left-radius: 1vh;
    border-bottom-right-radius: 1vh;
}

.device-s10 .mainmenu{
    height: 0.4vh;
    width: 5vh;
    margin-left: 10vh;
    position: absolute;
    top: 48vh;
    background: #ffffff;
    z-index: 20;
    border-radius: 1vh;
    border-color: #000000;
    border-style: solid;
    border-width: 0.054vh;
}

.device-s10 .powerbtn{
    height: 4vh;
    width: 0.3vh;
    margin-left: 24.35vh;
    position: absolute;
    margin-top: 10vh;
    background: #303030;
    z-index: 20;
    border-top-right-radius: 1vh;
    border-bottom-right-radius: 1vh;
}

.device-s10 .volumebtn{
    height: 8vh;
    width: 0.3vh;
    margin-left: -0.3vh;
    position: absolute;
    margin-top: 6vh;
    background: #303030;
    z-index: 20;
    border-top-left-radius: 1vh;
    border-bottom-left-radius: 1vh;
}

.device-s10 .addbtn{
    height: 4vh;
    width: 0.3vh;
    margin-left: -0.3vh;
    position: absolute;
    margin-top: 18vh;
    background: #303030;
    z-index: 20;
    border-top-left-radius: 1vh;
    border-bottom-left-radius: 1vh;
}
</style>