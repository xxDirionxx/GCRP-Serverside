<template>
    <v-ons-page>
        <v-ons-list>
            <div class="gpsmenu1">
                <h6 style="color: #0076ff; padding-left: 0.4vh !important; padding-top: 0vh !important; display: inline-block; line-height: 3.5vh;" v-on:click="homescreen">
                    <ons-icon icon="md-arrow-back" style="font-size: 2vh; margin: 0; padding: 0; line-height: 0vh; color: #1e88e5"></ons-icon>
                </h6>
                <!--<input style="display: inline-block; width: 18.3vh; margin-left: 0.4vh; margin-top: 0.5vh; height: 3vh; background-color: #F5F5F5; color: #000000; border-radius: 0.75vh; border-bottom: 0.25vh none #0076ff;" placeholder="Zieladresse suchen" v-model="filter" type="text" />-->
                <div class="segment" style="width: 280px; margin: 0 auto;">
                    <div class="segment__item">
                        <input type="radio" class="segment__input" name="segment-a" checked v-on:click="requestGPS(0)">
                        <div class="segment__button">Orte</div>
                    </div>
                    <div class="segment__item">
                        <input type="radio" class="segment__input" name="segment-a" v-on:click="requestGPS(1)">
                        <div class="segment__button">Fahrzeuge</div>
                    </div>
                </div>
            </div>

            <div style="padding-top: 0.5vh; background-color: #efeff4" v-for="category in categories" :key="category.name">
                <h5 style="color: #696969; margin-bottom: 0.5vh !important;">{{category.name}}</h5>
                <div style="border-bottom: 0.13vh solid #F5F5F5;">
                    <div class="listhover" style="border-top: 0.07vh solid #F5F5F5;" v-for="gps in search(category.locations)" :key="gps.name" v-on:click="setgps(gps)">
                        <p class="fas fa-map-marker-alt" style="display: inline-block; padding-left: 1.25vh; color: #696969"></p>
                        <h6 style="display: inline-block; padding-left: 0.5vh !important; padding-top: 0.75vh !important; padding-bottom: 0.75vh !important; color: #000000;">{{trimname(gps.name)}}</h6>
                    </div>
                </div>
            </div>

            <div style="padding-top: 0.5vh; background-color: #efeff4" v-if="categories.length == 0">
                <h5 style="color: #696969; padding-left: 0vh !important; padding-top: 10vh !important; text-align: center;">{{result}}</h5>
            </div>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
import Apps from "../../apps"
var filter;
var timer;

export default Apps.register({
    name: "GpsApp",
    data() {
        return {
            categories: [],
            filter: '',
            result: 'Wird geladen..',
        }
    },
    methods: {
        search(data){
            filter = this.filter.trim().toLowerCase();
            if (filter === '') return data;
            return data.filter(function(s) {
                return s.name.toLowerCase().indexOf(filter) >= 0;
            });
        },
        requestGPS(num){
            if(num == 0){
                this.triggerServer("requestGpsLocations")
                timer = setInterval(this.noresponse,3000)
                this.result = "Wird geladen.."
                console.log("requestGpsLocations")
            }else if(num == 1){
                this.triggerServer("requestVehicleGps")
                timer = setInterval(this.noresponse,3000)
                this.result = "Wird geladen.."
                console.log("requestVehicleGps")
            }
        },
        trimname(name){
            return name.substr(0, 29);
        },
        homescreen(){
            this.pageStack.pop()
            console.log("close")
        },
        gpsLocationsResponse(response) {
            this.categories = JSON.parse(response)
            clearInterval(timer)
            console.log("responseresponseresponse")
        },
        setgps(location){
            this.trigger("setGpsCoordinates", JSON.stringify({ x:location.x, y:location.y }))
        },
        noresponse(){
            clearInterval(timer)
            this.result = "Keine Ergebnisse"
        }
    },
    mounted() {
        this.triggerServer("requestGpsLocations")
        timer = setInterval(this.noresponse,3000)
        this.result = "Wird geladen.."
        console.log("requestGpsLocations")
    },
    props: ['pageStack']
})
</script>

<style lang="scss" scoped>
    .gpsmenu1{
        width: 100%;
        height: 6.7vh;
        border-bottom: 0.1vh solid #F5F5F5;
    }
    .listhover{
        background-color: #ffffff;
    }
    .listhover:hover{
        background-color: #F5F5F5;
    }
    .segment {
        width: 100% !important;
        &__item{
            height: 3.2vh;
        }
        &__item:hover {
            background-color: #ffffff;
            color: #000000 !important;

            .segment__button {
                color: #000000;
            }
        }
    
        &__button {
            border-radius: none;
            border: 0px;
            color: #696969;
            font-size: 1.25vh !important;
        }
        :checked{
            border-bottom: 0.25vh solid #0076ff;
        }
        :checked + &__button {
            background-color: #ffffff;
            color: #000000;
        }
    }
    h5{
        margin: 0 !important;
        color: #000000;
        font-size: 1.5vh;
        padding-left: 1vh !important;
        padding-top: 1vh !important;
    }
    h6{
        margin: 0 !important;
        color: #A9A9A9;
        font-size: 1.25vh;
        padding-left: 13.5vh !important;
        padding-top: 1vh !important;
    }
    h6:hover{
        color: #000000;
    }

</style>