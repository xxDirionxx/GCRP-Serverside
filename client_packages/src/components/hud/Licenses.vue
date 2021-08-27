<template>
    <div class="license" v-if="show">
        <div class="head">
            <div style="display: grid;">
                <h3>Lizenzen von <span style="font-weight: 600">{{ name.replace("_", " ") }}</span></h3>
            </div>
        </div>
        <div class="content">
            <div class="licenses">
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-first-aid color-text" :class="{'green': firstaid == 1 || firstaid == 2, 'red': firstaid == 0}"></i>
                        <h5 class="lizenzName">
                            EHK-Lizenz<br>
                            <small v-if="firstaid == 1" class="color-text green">Vorhanden</small>
                            <small v-if="firstaid == 2" class="color-text green">Vorhanden</small>
                            <small v-if="firstaid == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns: repeat(2, 2fr); display: grid">
                        <i class="fas fa-bomb color-text" :class="{'green': gunlicense == 1 || gunlicense == 2, 'red': gunlicense == 0}"></i>
                        <h5 class="lizenzName">
                            Waffenschein<br>
                            <small v-if="gunlicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="gunlicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="gunlicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-car color-text" :class="{'green': driverlicense == 1 || driverlicense == 2, 'red': driverlicense == 0}"></i>
                        <h5 class="lizenzName">
                            PKW-Lizenz<br>
                            <small v-if="driverlicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="driverlicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="driverlicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns:repeat(2, 2fr); display: grid">
                        <i class="fas fa-truck color-text" :class="{'green': trucklicense == 1 || trucklicense == 2, 'red': trucklicense == 0}"></i>
                        <h5 class="lizenzName">
                            LKW-Lizenz<br>
                            <small v-if="trucklicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="trucklicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="trucklicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-motorcycle color-text" :class="{'green': motorcyclelicense == 1 || motorcyclelicense == 2, 'red': motorcyclelicense == 0}"></i>
                        <h5 class="lizenzName">
                            Motorrad-Lizenz<br>
                            <small v-if="motorcyclelicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="motorcyclelicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="motorcyclelicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns: repeat(2, 2fr); display: grid">
                        <i class="fas fa-ship color-text" :class="{'green': boatlicense == 1 || boatlicense == 2, 'red': boatlicense == 0}"></i>
                        <h5 class="lizenzName">
                            Boot-Lizenz<br>
                            <small v-if="boatlicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="boatlicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="boatlicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-plane color-text" :class="{'green': flyinglicensea == 1 || flyinglicensea == 2, 'red': flyinglicensea == 0}"></i>
                        <h5 class="lizenzName">
                            Flugschein A<br>
                            <small v-if="flyinglicensea == 1" class="color-text green">Vorhanden</small>
                            <small v-if="flyinglicensea == 2" class="color-text green">Vorhanden</small>
                            <small v-if="flyinglicensea == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns: repeat(2, 2fr); display: grid">
                        <i class="fas fa-plane color-text" :class="{'green': flyinglicenseb == 1 || flyinglicenseb == 2, 'red': flyinglicenseb == 0}"></i>
                        <h5 class="lizenzName">
                            Flugschein B<br>
                            <small v-if="flyinglicenseb == 1" class="color-text green">Vorhanden</small>
                            <small v-if="flyinglicenseb == 2" class="color-text green">Vorhanden</small>
                            <small v-if="flyinglicenseb == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-taxi color-text" :class="{'green': taxilicense == 1 || taxilicense == 2, 'red': taxilicense == 0}"></i>
                        <h5 class="lizenzName">
                            Taxi-Lizenz<br>
                            <small v-if="taxilicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="taxilicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="taxilicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns: repeat(2, 2fr); display: grid">
                        <i class="fas fa-bus color-text" :class="{'green': passengertransportlicense == 1, 'green': passengertransportlicense == 2, 'red': passengertransportlicense == 0}"></i>
                        <h5 class="lizenzName">
                            PBS<br>
                            <small v-if="passengertransportlicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="passengertransportlicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="passengertransportlicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license">
                    <div style="grid-template-columns: 1fr 2fr; display: grid">
                        <i class="fas fa-briefcase color-text" :class="{'green': lawyerlicense == 1, 'green': lawyerlicense == 2, 'red': lawyerlicense == 0}"></i>
                        <h5 class="lizenzName">
                            Anwalts-Lizenz<br>
                            <small v-if="lawyerlicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="lawyerlicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="lawyerlicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
                <div class="single-license" style="margin-left: -13vh">
                    <div style="grid-template-columns: repeat(2, 2fr); display: grid">
                        <i class="fas fa-gavel color-text" :class="{'green': registryofficelicense == 1, 'green': registryofficelicense == 2, 'red': registryofficelicense == 0}"></i>
                        <h5 class="lizenzName">
                            Standesamt-Lizenz<br>
                            <small v-if="registryofficelicense == 1" class="color-text green">Vorhanden</small>
                            <small v-if="registryofficelicense == 2" class="color-text green">Vorhanden</small>
                            <small v-if="registryofficelicense == 0" class="color-text red">Nicht Vorhanden</small>
                        </h5>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Components from "../components"

    export default Components.register({
        name: "Licenses",
        data() {
            return {
                name: '',
                firstaid: null,
                gunlicense: null,
                driverlicense: null,
                trucklicense: null,
                motorcyclelicense: null,
                boatlicense: null,
                flyinglicensea: null,
                flyinglicenseb: null,
                taxilicense: null,
                passengertransportlicense: null,
                lawyerlicense: null,
                registryofficelicense: null,
                timeout: '',
                show: false
            }
        },
        methods: {
            remove: function () {
                this.show = false
            },
            showLic: function (name, firstaid, gunlicense, driverlicense, trucklicense, motorcyclelicense, boatlicense, flyinglicensea, flyinglicenseb, taxilicense, passengertransportlicense, lawyerlicense, registryofficelicense) {
                this.name = name
                this.firstaid = firstaid
                this.gunlicense = gunlicense
                this.driverlicense = driverlicense
                this.trucklicense = trucklicense
                this.motorcyclelicense = motorcyclelicense
                this.boatlicense = boatlicense
                this.flyinglicensea = flyinglicensea
                this.flyinglicenseb = flyinglicenseb
                this.taxilicense = taxilicense
                this.passengertransportlicense = passengertransportlicense
                this.lawyerlicense = lawyerlicense
                this.registryofficelicense = registryofficelicense
                this.show = true
                this.timeout = setTimeout(this.remove, 5000)
            }
        }
    })
</script>

<style lang="scss" scoped>
    .license {
        border-radius: 1vh;
        box-shadow: 5px 1px 10px -1px rgba(0, 0, 0, 0.4);
        margin-left: auto;
        margin-right: auto;
        width: 46vh;
        height: 45vh;
        background-color: #eee;

        .head {
            border-top-right-radius: 1vh;
            border-top-left-radius: 1vh;
            border-top: 1.3vh solid #FF9800;
            border-bottom: 0.5vh solid #FF9800;
            h3 {
                margin-top: 1.5vh;
                margin-bottom: 1.5vh;
                font-size: 1.5vh;
                color: #37474F;
                font-weight: 400;
                margin-left: 3vh;
                text-align: left;
            }
            i {
                font-size: 3.5vh;
                color: #37474F;
                text-align: right;
                margin-right: 2.5vh;
                margin-top: 1.5vh;
            }
            i:hover {
                color: #263238;
                transition: color 0.5s;
            }
        }
        .content {
            margin-top: 1.5vh;
            .head-img img {
                width: 10rem;
                height: 10rem;
            }
            .licenses {
                margin-top: 1vh;
                display: grid;
                grid-template-columns: repeat(2, 1fr);
                .single-license {
                    padding-bottom: 1vh;
                    i {
                        font-size: 3vh;
                        text-align: right;
                        margin-top: 0.5vh;
                        text-shadow: 0.15vh 0.15vh #000;
                    }
                    h5{
                        margin: 0.5vh 0.5vh 1vh;

                    }
                }
            }
        }
    }
    .lizenzName {
        padding-left: 1vh; 
        font-size: 1.6vh;
    }
</style>

<!-- components.get("Windows").show("Licenses", '{"name": "James_Ling", "firstaid": false, "gunlicense": true, "driverlicense": true, "trucklicense": false, "motorcyclelicense": true, "boatlicense": true, "flyinglicensea": false, "flyinglicenseb": false, "taxilicense": true, "passengertransportlicense": true}') -->